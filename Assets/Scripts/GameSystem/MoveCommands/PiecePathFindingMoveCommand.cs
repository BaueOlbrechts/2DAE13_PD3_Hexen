using BoardSystem;
using GameSystem.Models;
using GameSystem.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameSystem.MoveCommands
{
    public class PiecePathFindingMoveCommand
    {
        public  List<HexTile> Tiles(Board<BoardPiece> board, BoardPiece piece, HexTile toTile)
        {
            var fromTile = board.TileOf(piece);

            List<HexTile> NeighbourStrategy(HexTile centerTile) => Neighbours(centerTile, board);

            float DistanceStrategy(HexTile ft, HexTile tt) => Distance(ft, tt, board);

            var pf = new AStarPathFinding<HexTile>(NeighbourStrategy, DistanceStrategy, DistanceStrategy);

            return pf.Path(fromTile, toTile);
        }

        private List<HexTile> Neighbours(HexTile tile, Board<BoardPiece> board)
        {
            var neighbours = new List<HexTile>();
            var position = tile.HexPosition;

            var upPosition = position;
            upPosition.R += 1;
            var upTile = board.TileAt(upPosition);
            if (upTile != null && board.PieceAt(upTile) == null)
                neighbours.Add(upTile);

            var upRightPosition = position;
            upRightPosition.Q += 1;
            var upRightTile = board.TileAt(upRightPosition);
            if (upRightTile != null && board.PieceAt(upRightTile) == null)
                neighbours.Add(upRightTile);

            var downRightPosition = position;
            downRightPosition.Q += 1;
            downRightPosition.R -= 1;
            var downRightTile = board.TileAt(downRightPosition);
            if (downRightTile != null && board.PieceAt(downRightTile) == null)
                neighbours.Add(downRightTile);

            var downPosition = position;
            downPosition.R -= 1;
            var downTile = board.TileAt(downPosition);
            if (downTile != null && board.PieceAt(downTile) == null)
                neighbours.Add(downTile);

            var downLeftPosition = position;
            downRightPosition.Q -= 1;
            var downLeftTile = board.TileAt(downLeftPosition);
            if (downLeftTile != null && board.PieceAt(downLeftTile) == null)
                neighbours.Add(downLeftTile);

            var UpLeftPosition = position;
            UpLeftPosition.Q -= 1;
            UpLeftPosition.R += 1;
            var upLeftTile = board.TileAt(UpLeftPosition);
            if (upLeftTile != null && board.PieceAt(upLeftTile) == null)
                neighbours.Add(upLeftTile);

            return neighbours;
        }

        private float Distance(HexTile fromTile, HexTile toTile, Board<BoardPiece> board)
        {
            var fromPosition = fromTile.HexPosition;
            var toPosition = toTile.HexPosition;

            var xDistance = Mathf.Abs(fromPosition.Q - toPosition.Q);
            var yDistance = Mathf.Abs(fromPosition.Q + fromPosition.R - toPosition.Q - toPosition.R);
            var zDistance = Mathf.Abs(fromPosition.R - toPosition.R) / 2;

            var totalDistance = xDistance + yDistance + zDistance;
            return totalDistance;
        }
    }
}
