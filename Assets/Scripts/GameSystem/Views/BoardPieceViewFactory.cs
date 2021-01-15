using BoardSystem;
using GameSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameSystem.Views
{
    [CreateAssetMenu(fileName = "DefaultBoardPieceViewFactory", menuName = "GameSystem/BoardPieceViewFactory")]
    public class BoardPieceViewFactory : ScriptableObject
    {
        //[SerializeField]
        //private List<BoardPieceView> _darkChessPieceViews = default;
        //
        //[SerializeField]
        //private List<BoardPieceView> _lightChessPieceViews = default;
        //
        //[SerializeField]
        //private List<string> _movementNames = new List<string>();
        [SerializeField]
        private BoardPieceView _prefab = default;

        [SerializeField]
        private PositionHelper _positionHelper = null;


        public BoardPieceView CreateBoardPieceView(Board<BoardPiece> board, BoardPiece model)
        {
            //var list = model.IsLight ? _lightChessPieceViews : _darkChessPieceViews;
            //var index = _movementNames.IndexOf(model.MovementName);

            //var prefab = list[index]; ;
            var boardPieceView = GameObject.Instantiate<BoardPieceView>(_prefab);

            var tile = board.TileOf(model);
            boardPieceView.transform.position = _positionHelper.ToWorldPosition(board, tile.HexPosition);
            //boardPieceView.name = $"Spawned ChessPiece ( {model.MovementName} )";
            boardPieceView.Model = model;

            return boardPieceView;
        }
    }
}
