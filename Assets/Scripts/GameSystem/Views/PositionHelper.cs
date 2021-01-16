using BoardSystem;
using GameSystem.Models;
using UnityEngine;

namespace GameSystem.Views
{
    [CreateAssetMenu(fileName = "DefaultPositionHelper", menuName = "GameSystem/PositionHelper")]
    public class PositionHelper : ScriptableObject
    {
        [SerializeField]
        private float _hexRadius = 1;

        public float HexRadius => _hexRadius;

        public HexPosition ToHexPosition(Board<BoardPiece> board, Vector3 worldPosition)
        {
            var q = worldPosition.x / (HexRadius * (3f / 2f));

            var r = worldPosition.z / (Mathf.Sqrt(3) * HexRadius) - q / 2f;


            var boardPosition = new HexPosition
            {
                Q = (int)q,
                R = Mathf.RoundToInt(r)
            };

            return boardPosition;
        }

        public Vector3 ToWorldPosition(Board<BoardPiece> board, HexPosition hexPosition)
        {
            var x = HexRadius * (3f / 2f) * hexPosition.Q;
            var z = HexRadius * (hexPosition.Q * (Mathf.Sqrt(3) / 2f) + Mathf.Sqrt(3) * hexPosition.R);

            var tilePosition = new Vector3(x, 0, z);
            return tilePosition;
        }
    }
}
