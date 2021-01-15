using BoardSystem;
using GameSystem.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystem.Views
{
    public class BoardView : MonoBehaviour
    {
        [SerializeField]
        private HexTileViewFactory _hexTileViewFactory;
        
        [SerializeField]
        private BoardPieceViewFactory _boardPieceViewFactory = null;

        private Board<BoardPiece> _model;

        public Board<BoardPiece> Model
        {
            set
            {
                if (_model != null)
                    _model.PiecePlaced -= OnPiecePlaced;
                
                _model = value;
                
                if (_model != null)
                    _model.PiecePlaced += OnPiecePlaced;
            }

            get => _model;
        }

        private void OnDestroy()
        {
            Model = null;
        }

        private void OnPiecePlaced(object sender, PiecePlacedEventArgs<BoardPiece> e)
        {
            var board = sender as Board<BoardPiece>;
            var piece = e.Piece;
        
            _boardPieceViewFactory.CreateBoardPieceView(board, piece);
        }
    }
}