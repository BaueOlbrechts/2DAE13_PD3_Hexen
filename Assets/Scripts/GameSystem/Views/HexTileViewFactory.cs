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
    [CreateAssetMenu(fileName = "DefaultHexTileViewFactory", menuName = "GameSystem/HexTileViewFactory")]
    public class HexTileViewFactory : ScriptableObject
    {
        [SerializeField]
        private HexTileView _hexTileView = default;

        [SerializeField]
        private PositionHelper _positionHelper = default;

        public HexTileView CreateHexTileView(Board<BoardPiece> board, HexTile hexTile, Transform parent)
        {
            var position = _positionHelper.ToWorldPosition(board, hexTile.HexPosition);
            var hexTileView = Instantiate(_hexTileView, position, Quaternion.identity, parent);
            hexTileView.name = $"HexTile {hexTile.HexPosition.Q} {hexTile.HexPosition.R}";
            return hexTileView;
        }
    }
}
