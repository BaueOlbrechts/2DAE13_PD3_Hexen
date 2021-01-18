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

            //Leave pieces next to the player
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
            
            //Sort pieces according to distance
            for (int i = 0; i < piecesToCheck.Count; i++)
            {
                for (int j = 0; j < piecesToCheck.Count; j++)
                {
                    if (DistanceToTile(_board.TileOf(piecesToCheck[j]), _board.TileOf(_playerPiece)) > DistanceToTile(_board.TileOf(piecesToCheck[i]), _board.TileOf(_playerPiece)))
                    {
                        var temp = piecesToCheck[i];
                        piecesToCheck[i] = piecesToCheck[j];
                        piecesToCheck[j] = temp;
                    }
                }
            }

            //Try to get the closest available tile (or stay if there are none)
            for (int i = 0; i < piecesToCheck.Count; i++)
            {
                var piece = piecesToCheck[i];
                HexTile pieceTile = _board.TileOf(piece);

                if (availableTilesAroundPlayer.Count > 0)
                {
                    HexTile nearestTile = availableTilesAroundPlayer[0];
                    float distance = Mathf.Infinity;

                    foreach (var tile in availableTilesAroundPlayer)
                    {
                        float totalDistance = DistanceToTile(pieceTile, tile);
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

        private static float DistanceToTile(HexTile fromTile, HexTile toTile)
        {
            var fromPosition = fromTile.HexPosition;
            var toPosition = toTile.HexPosition;

            var xDistance = Mathf.Abs(fromPosition.Q - toPosition.Q);
            var yDistance = Mathf.Abs(fromPosition.Q + fromPosition.R - toPosition.Q - toPosition.R);
            var zDistance = Mathf.Abs(fromPosition.R - toPosition.R) / 2;

            var totalDistance = xDistance + yDistance + zDistance;
            return totalDistance;
        }

        private void MoveToPlayerState()
        {
            StateMachine.MoveTo(GameStates.Player);
        }
    }
}
