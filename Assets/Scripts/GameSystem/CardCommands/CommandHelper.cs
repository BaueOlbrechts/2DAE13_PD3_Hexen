using BoardSystem;
using GameSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameSystem.CardCommands
{
    public class CommandHelper
    {
        public delegate bool Validator(Board<BoardPiece> board, BoardPiece boardPiece, HexTile toTile);

        private Board<BoardPiece> _board;
        private BoardPiece _boardPiece;
        private List<HexTile> _tiles = new List<HexTile>();

        public CommandHelper(Board<BoardPiece> board, BoardPiece boardPiece)
        {
            _board = board;
            _boardPiece = boardPiece;
        }

        public CommandHelper Up(int steps = int.MaxValue, params Validator[] validators)
        {
            return Collect(0, 1, steps, validators);
        }

        public CommandHelper UpRight(int steps = int.MaxValue, params Validator[] validators)
        {
            return Collect(1, 0, steps, validators);
        }

        public CommandHelper DownRight(int steps = int.MaxValue, params Validator[] validators)
        {
            return Collect(1, -1, steps, validators);
        }

        public CommandHelper Down(int steps = int.MaxValue, params Validator[] validators)
        {
            return Collect(0, -1, steps, validators);
        }

        public CommandHelper DownLeft(int steps = int.MaxValue, params Validator[] validators)
        {
            return Collect(-1, 0, steps, validators);
        }

        public CommandHelper UpLeft(int steps = int.MaxValue, params Validator[] validators)
        {
            return Collect(-1, 1, steps, validators);
        }

        public CommandHelper CollectDirectionNeighbours(HexPosition directionHexPos, int steps = int.MaxValue, params Validator[] validators)
        {
            Vector2[] directions = new[] { new Vector2(0, 1), new Vector2(1, 0), new Vector2(1, -1), new Vector2(0, -1), new Vector2(-1, 0), new Vector2(-1, 1) };
            var direction = new Vector2(directionHexPos.Q, directionHexPos.R);

            var idx = Array.IndexOf(directions, direction);

            if (idx == -1)
            {
                return AllDirections(steps);
            }

            List<int> indexes = new List<int>();
            indexes.Add(idx);

            if (idx == 0)
            {
                indexes.Add(directions.Length - 1);
                indexes.Add(idx + 1);
            }
            else if (idx == directions.Length - 1)
            {
                indexes.Add(idx - 1);
                indexes.Add(0);
            }
            else
            {
                indexes.Add(idx - 1);
                indexes.Add(idx + 1);
            }

            var newHelper = new CommandHelper(_board, _boardPiece);

            foreach (var index in indexes)
            {
                var dir = directions[index];
                newHelper.Collect((int)dir.x, (int)dir.y, steps);
            }

            return newHelper;
        }

        public CommandHelper AllDirections(int steps = int.MaxValue, params Validator[] validators)
        {
            return Up(steps)
                    .UpRight(steps)
                    .DownRight(steps)
                    .Down(steps)
                    .DownLeft(steps)
                    .UpLeft(steps);
        }

        public CommandHelper Collect(int q, int r, int steps = int.MaxValue, params Validator[] validators)
        {
            HexPosition MoveNext(HexPosition position)
            {
                position.Q += q;
                position.R += r;

                return position;
            }

            var startTile = _board.TileOf(_boardPiece);
            var startPosition = startTile.HexPosition;

            var nextPosition = MoveNext(startPosition);

            int currentStep = 0;
            var canContinue = true;
            while (canContinue && currentStep < steps)
            {
                var nextTile = _board.TileAt(nextPosition);
                if (nextTile == null)
                {
                    canContinue = false;
                    break;
                }

                //var nextPiece = _board.PieceAt(nextTile);
                //if (nextPiece != null)
                //    canContinue = false;

                if (validators.All(v => v(_board, _boardPiece, nextTile)))
                    _tiles.Add(nextTile);

                nextPosition = MoveNext(nextPosition);
                currentStep++;
            }

            return this;
        }

        public List<HexTile> GenerateTiles()
        {
            return _tiles;
        }

        public static bool CanCapture(Board<BoardPiece> board, BoardPiece boardPiece, HexTile toTile)
        {
            var other = board.PieceAt(toTile);
            return other != null && other != boardPiece;
        }

        public static bool IsEmpty(Board<BoardPiece> board, BoardPiece boardPiece, HexTile toTile)
        {
            var other = board.PieceAt(toTile);
            return other == null;
        }

        public static HexPosition DetermineHexDirection(HexTile startTile, HexTile cursorTile)
        {
            var startTileCubePos = startTile.CubePosition;
            var cursorTileCubePos = cursorTile.CubePosition;

            if (startTileCubePos == cursorTileCubePos)
                return default;

            var direction = cursorTileCubePos - startTileCubePos;

            if (!(direction.x == 0 || direction.y == 0 || direction.z == 0))
                return default;

            direction /= Mathf.Max(direction.x, direction.y, direction.z);

            return new HexPosition { Q = (int)direction.x, R = (int)direction.z };
        }
    }
}
