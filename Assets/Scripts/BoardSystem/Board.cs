using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BoardSystem
{
    public class PiecePlacedEventArgs<TPiece> : EventArgs where TPiece : class, IPiece<TPiece>
    {
        public TPiece Piece { get; }

        public PiecePlacedEventArgs(TPiece piece)
        {
            Piece = piece;
        }
    }

    public class Board<TPiece> where TPiece : class, IPiece<TPiece>
    {
        public event EventHandler<PiecePlacedEventArgs<TPiece>> PiecePlaced;

        private Dictionary<HexPosition, HexTile> _hexTiles = new Dictionary<HexPosition, HexTile>();
        private List<TPiece> _values = new List<TPiece>();
        private List<HexTile> _keys = new List<HexTile>();

        public List<HexTile> Tiles => _hexTiles.Values.ToList();

        public readonly int BoardRings;

        public Board(int boardRings)
        {
            BoardRings = boardRings;

            InitHexTiles();
        }

        public HexTile TileAt(HexPosition hexPosition)
        {
            if (_hexTiles.TryGetValue(hexPosition, out var foundValue))
                return foundValue;

            return null;
        }

        public TPiece PieceAt(HexTile hexTile)
        {
            //IndexOff returns -1 when not found
            var idx = _keys.IndexOf(hexTile);

            if (idx == -1)
                return default(TPiece);

            return _values[idx];
        }

        public HexTile TileOf(TPiece piece)
        {
            var idx = _values.IndexOf(piece);
            if (idx == -1)
                return null;

            return _keys[idx];
        }

        public TPiece Take(HexTile fromHexTile)
        {
            var idx = _keys.IndexOf(fromHexTile);
            if (idx == -1)
                return default(TPiece);

            var piece = _values[idx];

            _values.RemoveAt(idx);
            _keys.RemoveAt(idx);

            piece.Taken(this);
            return piece;
        }

        public void Move(HexTile fromHexTile, HexTile toHexTile)
        {
            var idx = _keys.IndexOf(fromHexTile);
            if (idx == -1)
                return;

            var toPiece = PieceAt(toHexTile);
            if (toPiece != null)
                return;

            _keys[idx] = toHexTile;

            var piece = _values[idx];
            piece.Moved(this, fromHexTile, toHexTile);
        }

        public void Place(HexTile toHexTile, TPiece piece)
        {
            if (_keys.Contains(toHexTile))
                return;

            if (_values.Contains(piece))
                return;

            _keys.Add(toHexTile);
            _values.Add(piece);

            OnPiecePlaced(new PiecePlacedEventArgs<TPiece>(piece));
        }

        public void UnHighlight(List<HexTile> hexTiles)
        {
            foreach (var hexTile in hexTiles)
            {
                hexTile.IsHighlighted = false;
            }
        }

        public void Highlight(List<HexTile> hexTiles)
        {
            foreach (var hexTile in hexTiles)
            {
                hexTile.IsHighlighted = true;
            }
        }

        protected virtual void OnPiecePlaced(PiecePlacedEventArgs<TPiece> args)
        {
            EventHandler<PiecePlacedEventArgs<TPiece>> handler = PiecePlaced;
            handler?.Invoke(this, args);
        }

        private void InitHexTiles()
        {
            for (int q = -BoardRings; q <= BoardRings; q++)
            {
                for (int r = -BoardRings; r <= BoardRings; r++)
                {
                    var combinedNumber = Mathf.Abs(q + r);

                    if (combinedNumber <= BoardRings)
                        _hexTiles.Add(new HexPosition { Q = q, R = r }, new HexTile(q, r));
                }
            }
        }
    }
}
