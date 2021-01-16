using BoardSystem;
using GameSystem.Models;
using MoveSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameSystem.CardCommands
{
    public abstract class AbstractCardCommand : ICardCommand<BoardPiece>
    {
        public virtual bool CanExecute(Board<BoardPiece> board, HexTile playerTile, HexTile cursorTile)
        {
            var validTiles = HexTiles(board, playerTile, cursorTile);
            return validTiles.Count > 0;
        }

        public abstract void Execute(Board<BoardPiece> board, BoardPiece piece, HexTile toTile);

        public abstract List<HexTile> HexTiles(Board<BoardPiece> board, HexTile playerTile, HexTile cursorTile);
    }
}
