using BoardSystem;
using GameSystem.CardCommands;
using GameSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameSystem.States
{
    public class EnemyGameState : GameStateBase
    {
        private Board<BoardPiece> _board;
        private BoardPiece _playerPiece = null;

        public EnemyGameState(Board<BoardPiece> board, BoardPiece playerPiece)
        {
            _board = board;
            _playerPiece = playerPiece;
        }

        public override void OnEnter()
        {
            MovePiecesToTile();
            SetNewPieceToTiles();
            MoveToPlayerState();
        }



        private void MovePiecesToTile()
        {
            var piecesToMove = _board.Pieces;
            piecesToMove.Remove(_playerPiece);

            foreach (var piece in piecesToMove)
            {
                if (piece.ToTile != null)
                    _board.Move(_board.TileOf(piece), piece.ToTile);
            }
        }

        private void SetNewPieceToTiles()
        {
            List<HexTile> availableTilesAroundPlayer = new CommandHelper(_board, _playerPiece).AllDirections(1).GenerateTiles();
            var piecesToCheck = _board.Pieces;
            piecesToCheck.Remove(_playerPiece);

            foreach (var piece in _board.Pieces)
            {
                HexTile pieceTile = _board.TileOf(piece);

                if (availableTilesAroundPlayer.Contains(pieceTile))
                {
                    piece.SetNewToTile(pieceTile);
                    availableTilesAroundPlayer.Remove(pieceTile);
                    piecesToCheck.Remove(piece);
                }
            }


            foreach (var piece in piecesToCheck)
            {
                HexTile pieceTile = _board.TileOf(piece);
                var fromPosition = pieceTile.HexPosition;

                if (availableTilesAroundPlayer.Count > 0)
                {
                    HexTile nearestTile = availableTilesAroundPlayer[0];
                    float distance = Mathf.Infinity;

                    foreach (var tile in availableTilesAroundPlayer)
                    {
                        var toPosition = tile.HexPosition;

                        var xDistance = Mathf.Abs(fromPosition.Q - toPosition.Q);
                        var yDistance = Mathf.Abs(fromPosition.R - toPosition.R);

                        var totalDistance = xDistance + yDistance;
                        if (totalDistance < distance)
                        {
                            distance = totalDistance;
                            nearestTile = tile;
                        }
                    }

                    piece.SetNewToTile(nearestTile);
                    availableTilesAroundPlayer.Remove(nearestTile);
                }
                else
                    piece.SetNewToTile(pieceTile);
            }
        }

        private void MoveToPlayerState()
        {
            StateMachine.MoveTo(GameStates.Player);
        }
    }
}
