using GameSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameSystem.Views
{
    [SelectionBase]
    public class BoardPieceView : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private PositionHelper _positionHelper = null;

        [SerializeField]
        private bool _isPlayer = false;
        
        private BoardPiece _model;

        public bool IsPlayer => _isPlayer;
        

        public BoardPiece Model
        {
            get => _model;
            internal set
            {
                if (_model != null)
                {
                    _model.BoardPieceMoved -= ModelMoved;
                    _model.BoardPieceTaken -= ModelTaken;
                }

                _model = value;

                if (_model != null)
                {
                    _model.BoardPieceMoved += ModelMoved;
                    _model.BoardPieceTaken += ModelTaken;
                }
            }
        }

        private void ModelTaken(object sender, EventArgs e)
        {
            //Debug.Log("Model destroyed");
            Destroy(gameObject);
        }

        private void ModelMoved(object sender, BoardPieceMovedEventArgs e)
        {
            var board = e.Board;
            var worldPosition = _positionHelper.ToWorldPosition(board, e.To.HexPosition);
            transform.position = worldPosition;
        }

        public void OnPointerClick(PointerEventData eventData)
        {

            var board = GameLoop.Instance.Board;
            var hexPos = _positionHelper.ToHexPosition(board, transform.position);
            Debug.Log($"Piece HexPosition {hexPos.Q} {hexPos.R}");
        }

        private void OnDestroy()
        {
            Model = null;
        }
    }
}

