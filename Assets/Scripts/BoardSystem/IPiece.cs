using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoardSystem
{
    public interface IPiece<TPiece> where TPiece : class, IPiece<TPiece>
    {
        void Moved(Board<TPiece> board, HexTile fromTile, HexTile toTile);
        void Taken(Board<TPiece> board);
    }
}
