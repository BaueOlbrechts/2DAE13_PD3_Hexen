using BoardSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameSystem.Models
{
    public class BoardPieceMovedEventArgs : System.EventArgs
    {
        public Board<BoardPiece> Board { get; }
        public HexTile From { get; }
        public HexTile To { get; }

        public BoardPieceMovedEventArgs(Board<BoardPiece> board, HexTile from, HexTile to)
        {
            Board = board;
            From = from;
            To = to;
        }
    }

    public class BoardPiece : IPiece<BoardPiece>
    {
        public event EventHandler<BoardPieceMovedEventArgs> BoardPieceMoved;
        public event EventHandler BoardPieceTaken;

        public bool HasMoved { get; set; }

        public bool IsPlayer { get; }
        public HexTile ToTile { get; internal set; } = null;

        public BoardPiece(bool isPlayer)
        {
            IsPlayer = isPlayer;
        }

        void IPiece<BoardPiece>.Moved(Board<BoardPiece> board, HexTile fromTile, HexTile toTile)
        {
            OnBoardPieceMoved(new BoardPieceMovedEventArgs(board, fromTile, toTile));
        }

        void IPiece<BoardPiece>.Taken(Board<BoardPiece> board)
        {
            OnBoardPieceTaken(EventArgs.Empty);
        }

        public void SetNewToTile(HexTile toTile)
        {
            ToTile = toTile;
        }

        protected virtual void OnBoardPieceMoved(BoardPieceMovedEventArgs arg)
        {
            EventHandler<BoardPieceMovedEventArgs> handler = BoardPieceMoved;
            handler?.Invoke(this, arg);
        }

        protected virtual void OnBoardPieceTaken(EventArgs arg)
        {
            EventHandler handler = BoardPieceTaken;
            handler?.Invoke(this, arg);
        }
    }
}
