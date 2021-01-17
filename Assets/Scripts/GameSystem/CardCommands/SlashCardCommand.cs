using BoardSystem;
using GameSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameSystem.CardCommands
{
    public class SlashCardCommand : AbstractCardCommand
    {
        public override void Execute(Board<BoardPiece> board, BoardPiece piece, HexTile toTile)
        {
            var tiles = GameLoop.Instance.CardManager.Tiles();

            foreach (var tile in tiles)
            {
                var toPiece = board.PieceAt(tile);
                if (toPiece != null)
                {
                    board.Take(tile);
                }
            }
        }

        public override List<HexTile> HexTiles(Board<BoardPiece> board, HexTile playerTile, HexTile cursorTile)
        {
            var validHexTiles = new List<HexTile>();

            if (cursorTile == null)
            {
                var startTileCubePos = playerTile.CubePosition;
                Vector3[] directions = new[] { new Vector3(1, -1, 0), new Vector3(1, 0, -1), new Vector3(0, 1, -1), new Vector3(-1, 1, 0), new Vector3(-1, 0, 1), new Vector3(0, -1, 1) };

                foreach (var dir in directions)
                {
                    var checkPos = startTileCubePos + dir;
                    var tile = board.TileAt(checkPos);
                    validHexTiles.Add(tile);
                }
            }
            else
            {
                var startTileCubePos = playerTile.CubePosition;
                var cursorTileCubePos = cursorTile.CubePosition;

                var direction = cursorTileCubePos - startTileCubePos;
                Vector3[] directions = new[] { new Vector3(1, -1, 0), new Vector3(1, 0, -1), new Vector3(0, 1, -1), new Vector3(-1, 1, 0), new Vector3(-1, 0, 1), new Vector3(0, -1, 1) };
                var idx = Array.IndexOf(directions, direction);

                if (idx == -1)
                {
                    foreach (var dir in directions)
                    {
                        var checkPos = startTileCubePos + dir;
                        var tile = board.TileAt(checkPos);
                        validHexTiles.Add(tile);
                    }
                    return validHexTiles;
                }

                List<int> indexes = new List<int>();
                indexes.Add(idx);

                if (idx == 0)
                {
                    indexes.Add(directions.Length - 1);
                    indexes.Add(idx + 1);
                }
                else if (idx == directions.Length - 1)
                {
                    indexes.Add(idx - 1);
                    indexes.Add(0);
                }
                else
                {
                    indexes.Add(idx - 1);
                    indexes.Add(idx + 1);
                }

                foreach(var index in indexes)
                {
                    var checkpos = startTileCubePos + directions[index];
                    var tile = board.TileAt(checkpos);
                    validHexTiles.Add(tile);
                }
            }

            return validHexTiles;
        }
    }
}