using BoardSystem;
using GameSystem.CardCommands;
using GameSystem.Models;
using GameSystem.MoveCommands;
using MoveSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameSystem.States
{
    public class PlayerGameState : GameStateBase
    {
        private ICardCommand<BoardPiece> _currentCardCommand = null;
        private BoardPiece _playerPiece = null;
        private HexTile _hoverTile = null;
        private CardManager<BoardPiece> _cardManager = null;
        private Board<BoardPiece> _board = null;
        private int _turnsBeforeEnemyTurn = 2;
        private int _currentTurn = 1;

        public BoardPiece PlayerPiece => _playerPiece;
        public Board<BoardPiece> Board => _board;

        public PlayerGameState(Board<BoardPiece> board, BoardPiece playerPiece, CardManager<BoardPiece> cardManager)
        {
            _board = board;
            _playerPiece = playerPiece;
            _cardManager = cardManager;
        }
        
        public override void OnEnter()
        {
            Board.CardUsed += OnCardUsed;

            _currentTurn = 1;
        }

        public override void OnExit()
        {
            Board.CardUsed -= OnCardUsed;

            _cardManager.SetTiles(null);
        }

        public override void HoverOver(HexTile hexTile)
        {
            if (_playerPiece != null && _currentCardCommand != null)
            {
                Board.UnHighlight(_cardManager.Tiles());

                _hoverTile = hexTile;

                Board.Highlight(_cardManager.SetTiles(_currentCardCommand.HexTiles(Board, _board.TileOf(PlayerPiece), _hoverTile)));
            }
            else if (_playerPiece != null && _currentCardCommand == null)
            {
                if (_cardManager.Tiles() != null)
                    Board.UnHighlight(_cardManager.Tiles());

                var piece = Board.PieceAt(hexTile);
                if (piece != null && piece != _playerPiece)
                {
                    Board.Highlight(_cardManager.SetTiles(new PiecePathFindingMoveCommand().Tiles(Board, piece, piece.ToTile)));
                }
            }
        }

        public override void SelectTile(HexTile hexTile)
        {
            if (_playerPiece != null && _currentCardCommand != null)
            {
                if (_cardManager.Tiles().Contains(hexTile))
                {
                    Board.UnHighlight(_cardManager.Tiles());

                    _currentCardCommand.Execute(Board, _playerPiece, _hoverTile, _cardManager.Tiles());

                    Board.OnCardUsed(new EventArgs());
                }
            }
        }

        public override void SelectCard(ICardCommand<BoardPiece> cardCommand)
        {
            if (_currentCardCommand != null)
                Board.UnHighlight(_cardManager.Tiles());

            _currentCardCommand = cardCommand;

            if (_currentCardCommand != null)
                Board.Highlight(_cardManager.SetTiles(_currentCardCommand.HexTiles(Board, _board.TileOf(PlayerPiece), _hoverTile)));
        }


        public void OnCardUsed(object sender, EventArgs e)
        {
            _hoverTile = null;
            _currentCardCommand = null;

            if (_currentTurn >= _turnsBeforeEnemyTurn)
            {
                MoveToEnemyGameState();
            }
            else
            {
                _currentTurn++;
                _cardManager.SetTiles(null);
            }
        }



        private void MoveToEnemyGameState()
        {
            StateMachine.MoveTo(GameStates.Enemy);
        }
    }
}
