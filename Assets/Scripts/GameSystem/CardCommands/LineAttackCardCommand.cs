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
    public class LineAttackCardCommand : AbstractCardCommand
    {
        public override void Execute(Board<BoardPiece> board, BoardPiece piece, HexTile toTile, List<HexTile> validTiles)
        {
            foreach (var tile in validTiles)
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
                validHexTiles = new CommandHelper(board, board.PieceAt(playerTile))
                    .AllDirections()
                    .GenerateTiles();

                /*
                //All tiles in lines
                var startTileCubePos = playerTile.CubePosition;
                Vector3[] directions = new[] { new Vector3(1, -1, 0), new Vector3(1, 0, -1), new Vector3(0, 1, -1), new Vector3(-1, 1, 0), new Vector3(-1, 0, 1), new Vector3(0, -1, 1) };

                foreach (var direction in directions)
                {
                    int N = 1;
                    bool canContinue = true;

                    while (canContinue)
                    {
                        var nextTileCubePos = startTileCubePos + direction * N;
                        var nextTile = board.TileAt(nextTileCubePos);

                        if (nextTile == null)
                        {
                            canContinue = false;
                            break;
                        }

                        validHexTiles.Add(nextTile);
                        N++;
                    }
                }
                */


                // Less workload with predetermined directions
                /*
                for (int x = -1; x <= 1; x++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        for (int z = -1; z <= 1; z++)
                        {
                            if (x == 0 && y == 0 && z == 0)
                                continue;

                            if (x + y + z != 0)
                                continue;

                            var startTileCubePos = playerTile.BlockPosition;
                            var direction = new Vector3(x, y, z);

                            int N = 1;
                            bool canContinue = true;

                            while (canContinue)
                            {
                                var nextTileCubePos = startTileCubePos + direction * N;
                                var nextTile = board.TileAt(nextTileCubePos);

                                if (nextTile == null)
                                {
                                    canContinue = false;
                                    break;
                                }

                                validHexTiles.Add(nextTile);
                                N++;
                            }
                        }
                    }
                }
                */
            }
            else
            {
                var direction = CommandHelper.DetermineHexDirection(playerTile, cursorTile);
                if (direction.Q == 0 && direction.R == 0)
                    return new CommandHelper(board, board.PieceAt(playerTile)).AllDirections().GenerateTiles();

                validHexTiles = new CommandHelper(board, board.PieceAt(playerTile)).Collect((int)direction.Q, (int)direction.R).GenerateTiles();

                /*
                //Line in direction of cursor
                var startTileCubePos = playerTile.CubePosition;
                var cursorTileCubePos = cursorTile.CubePosition;

                if (startTileCubePos == cursorTileCubePos)
                    return validHexTiles;

                var direction = cursorTileCubePos - startTileCubePos;

                if (!(direction.x == 0 || direction.y == 0 || direction.z == 0))
                    return validHexTiles;

                direction /= Mathf.Max(direction.x, direction.y, direction.z);

                int N = 1;
                bool canContinue = true;

                while (canContinue)
                {
                    var nextTileCubePos = startTileCubePos + direction * N;
                    var nextTile = board.TileAt(nextTileCubePos);

                    if (nextTile == null)
                    {
                        canContinue = false;
                        break;
                    }

                    validHexTiles.Add(nextTile);
                    N++;
                }
                */
            }

            return validHexTiles;
        }
    }
}
