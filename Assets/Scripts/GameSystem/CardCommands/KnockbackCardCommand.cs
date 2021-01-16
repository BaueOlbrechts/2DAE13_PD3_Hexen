using BoardSystem;
using GameSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameSystem.CardCommands
{
    public class KnockbackCardCommand : AbstractCardCommand
    {
        public override void Execute(Board<BoardPiece> board, BoardPiece piece, HexTile toTile)
        {
            
        }

        public override List<HexTile> HexTiles(Board<BoardPiece> board, HexTile playerTile, HexTile cursorTile)
        {
            var validHexTiles = new List<HexTile>();

            return validHexTiles;
        }
    }
}
