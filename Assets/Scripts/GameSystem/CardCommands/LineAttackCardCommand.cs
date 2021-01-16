using BoardSystem;
using GameSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameSystem.CardCommands
{
    public class LineAttackCardCommand : AbstractCardCommand
    {
        public override void Execute(Board<BoardPiece> board, BoardPiece piece, HexTile toTile)
        {
            
        }

        public override List<HexTile> HexTiles(Board<BoardPiece> board, HexTile playerTile, HexTile cursorTile)
        {
            var validHexTiles = new List<HexTile>();
            if (cursorTile == null)
            {
                //All tiles in lines
                validHexTiles.Add(board.TileOf(GameLoop.Instance.PlayerPiece));
            }
            else
            {
                //Line in direction of cursor
                validHexTiles.Add(cursorTile);
            }

            return validHexTiles;
        }
    }
}
