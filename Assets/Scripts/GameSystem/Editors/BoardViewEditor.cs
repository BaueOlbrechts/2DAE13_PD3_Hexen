using BoardSystem;
using GameSystem.Views;
using UnityEditor;
using UnityEngine;

namespace GameSystem.Editor
{
    [CustomEditor(typeof(BoardView))]
    public class BoardViewEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Create Hex Board"))
            {
                var boardView = target as BoardView;

                var hexTileViewFactorySp = serializedObject.FindProperty("_hexTileViewFactory");
                var hexTileViewFactory = hexTileViewFactorySp.objectReferenceValue as HexTileViewFactory;

                var board = GameLoop.Instance.Board;

                foreach (var hexTile in board.Tiles)
                {
                    hexTileViewFactory.CreateHexTileView(board, hexTile, boardView.transform);
                }
            }

            if (GUILayout.Button("Delete Hex Board"))
            {
                var boardView = target as BoardView;
                var go = boardView.gameObject;
                for (int i = go.transform.childCount - 1; i >= 0; i--)
                {
                    var child = go.transform.GetChild(i).gameObject;
                    DestroyImmediate(child);
                }
            }
        }
    }
}
