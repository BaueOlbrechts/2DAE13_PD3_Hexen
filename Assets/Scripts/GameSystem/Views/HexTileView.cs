using BoardSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameSystem.Views
{
    public class HexTileView : MonoBehaviour, IPointerClickHandler
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
                _model = value;
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

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log($"Tile HexPosition: {Model.HexPosition.Q} {Model.HexPosition.R}");
            _meshRenderer.material = _highlightMaterial;

        }

        private void ModelHighlightStatusChanged(object sender, EventArgs e)
        {
            if (Model.IsHighlighted)
                _meshRenderer.material = _highlightMaterial;
            else
                _meshRenderer.material = _originalMaterial;
        }
    }
}
