using System;
using UnityEngine;

namespace BoardSystem
{
    public class HexTile
    {
        public event EventHandler HighlightStatusChanged;

        private bool _isHighlighted = false;

        public bool IsHighlighted
        {
            get => _isHighlighted;
            internal set
            {
                _isHighlighted = value;
                OnHighlightStatusChanged(EventArgs.Empty);
            }
        }

        public HexPosition HexPosition { get; }
        public Vector3 BlockPosition { get; }


        public HexTile(int q, int r)
        {
            HexPosition = new HexPosition { Q = q, R = r };
            BlockPosition = new Vector3(q, -(q + r), r);
        }


        protected virtual void OnHighlightStatusChanged(EventArgs args)
        {
            EventHandler handler = HighlightStatusChanged;
            handler?.Invoke(this, args);
        }
    }
}
