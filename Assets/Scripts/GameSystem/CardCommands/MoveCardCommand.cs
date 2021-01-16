using BoardSystem;
using GameSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameSystem.CardCommands
{
    public class MoveCardCommand : AbstractCardCommand
    {
        public override void Execute(Board<BoardPiece> board, BoardPiece piece, HexTile toTile)
        {
            var toPiece = board.PieceAt(toTile);
            if (toPiece != null)
            {
                board.Take(toTile);
            }

            var fromTile = board.TileOf(piece);

            board.Move(fromTile, toTile);
        }

        public override List<HexTile> HexTiles(Board<BoardPiece> board, HexTile playerTile, HexTile cursorTile)
        {
            var validHexTiles = new List<HexTile>();

            if (cursorTile == null)
            {
                //Tile under player
                validHexTiles.Add(playerTile);
            }
            else
            {
                //Tile under cursor
                validHexTiles.Add(cursorTile);
            }
            return validHexTiles;
        }
    }
}
