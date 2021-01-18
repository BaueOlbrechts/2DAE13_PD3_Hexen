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
    public class KnockbackCardCommand : AbstractCardCommand
    {
        public override void Execute(Board<BoardPiece> board, BoardPiece piece, HexTile toTile)
        {
            var tiles = GameLoop.Instance.CardManager.Tiles();

            foreach (var tile in tiles)
            {
                var toPiece = board.PieceAt(tile);
                if (toPiece != null)
                {
                    var dir = tile.CubePosition - board.TileOf(piece).CubePosition;
                    var nextPos = tile.CubePosition + dir;
                    var nextTile = board.TileAt(new HexPosition { Q = (int)nextPos.x, R = (int)nextPos.z });
                    if (nextTile == null)
                    {
                        board.Take(tile);
                    }

                    board.Move(tile, nextTile);
                }
            }
        }

        public override List<HexTile> HexTiles(Board<BoardPiece> board, HexTile playerTile, HexTile cursorTile)
        {
            var validHexTiles = new List<HexTile>();

            if (cursorTile == null)
            {
                validHexTiles = new CommandHelper(board, board.PieceAt(playerTile)).AllDirections(1).GenerateTiles();

                /*
                var startTileCubePos = playerTile.CubePosition;
                Vector3[] directions = new[] { new Vector3(1, -1, 0), new Vector3(1, 0, -1), new Vector3(0, 1, -1), new Vector3(-1, 1, 0), new Vector3(-1, 0, 1), new Vector3(0, -1, 1) };

                foreach (var dir in directions)
                {
                    var checkPos = startTileCubePos + dir;
                    var tile = board.TileAt(checkPos);
                    if (tile != null)
                        validHexTiles.Add(tile);
                }
                */
            }
            else
            {
                var direction = CommandHelper.DetermineHexDirection(playerTile, cursorTile);
                if (direction.Q == 0 && direction.R == 0)
                    return  new CommandHelper(board, board.PieceAt(playerTile)).AllDirections(1).GenerateTiles();

                validHexTiles = new CommandHelper(board, board.PieceAt(playerTile)).CollectDirectionNeighbours(direction, 1).GenerateTiles();


                /*
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
                        if (tile != null)
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

                foreach (var index in indexes)
                {
                    var checkpos = startTileCubePos + directions[index];
                    var tile = board.TileAt(checkpos);
                    if (tile != null)
                        validHexTiles.Add(tile);
                }
                */
            }

            return validHexTiles;
        }
    }
}
