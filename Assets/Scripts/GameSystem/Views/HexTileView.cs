using BoardSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameSystem.Views
{
    public class HexTileView : MonoBehaviour, IPointerClickHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        private Material _highlightMaterial = null;

        private Material _originalMaterial;
        private MeshRenderer _meshRenderer;
        private HexTile _model;

        public HexTile Model
        {
            get => _model;
            set
            {
                if (_model != null)
                    _model.HighlightStatusChanged -= ModelHighlightStatusChanged;

                _model = value;

                if (_model != null)
                    _model.HighlightStatusChanged += ModelHighlightStatusChanged;
            }
        }

        private void Start()
        {
            _meshRenderer = GetComponentInChildren<MeshRenderer>();
            _originalMaterial = _meshRenderer.sharedMaterial;
        }

        private void OnDestroy()
        {
            Model = null;
        }

        private void ModelHighlightStatusChanged(object sender, System.EventArgs e)
        {
            if (Model.IsHighlighted)
                _meshRenderer.material = _highlightMaterial;
            else
                _meshRenderer.material = _originalMaterial;
        }


        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log($"Tile HexPosition: {Model.HexPosition.Q} {Model.HexPosition.R}");
        }

        public void OnDrop(PointerEventData eventData)
        {
            GameLoop.Instance.SelectTile(Model);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            //Debug.Log($"Tile HexPosition: {Model.HexPosition.Q} {Model.HexPosition.R} entered");
            GameLoop.Instance.HoverOver(Model);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            GameLoop.Instance.HoverOver(null);
        }
    }
}
