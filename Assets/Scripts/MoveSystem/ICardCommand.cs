using BoardSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoveSystem
{
    public interface ICardCommand<TPiece> where TPiece : class, IPiece<TPiece>
    {
        bool CanExecute(Board<TPiece> board, HexTile playerTile, HexTile cursorTile);

        List<HexTile> HexTiles(Board<TPiece> board, HexTile playerTile, HexTile cursorTile);

        void Execute(Board<TPiece> board, TPiece piece, HexTile toTile);
    }
}
