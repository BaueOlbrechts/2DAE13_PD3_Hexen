using GameSystem.Models;
using MoveSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameSystem.Views
{
    public class CardCommandView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private Vector3 _originalPosition = default;
        private Transform _originalParent = default;
        private Image _image = default;

        [SerializeField]
        private string _cardCommandName = null;
        public string CardCommandName => _cardCommandName;

        public ICardCommand<BoardPiece> Model { get; set; }

        private void Start()
        {
            _originalParent = gameObject.transform.parent;
            _image = gameObject.GetComponent<Image>();
            Model = GameLoop.Instance.CardManager.GetCardCommand(CardCommandName);
        }


        public void OnBeginDrag(PointerEventData eventData)
        {
            _originalPosition = transform.position;
            GameLoop.Instance.SelectCard(Model);
            _image.raycastTarget = false;
            GameLoop.Instance.CardUsed += OnCardGetsUsed;
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            var go = eventData.pointerCurrentRaycast.gameObject;

            if (go)
                Debug.Log($"Over object {go.name}, but unable to execute");
            else
                Debug.Log("Not over object");

            transform.SetParent(_originalParent);
            transform.position = _originalPosition;
            _image.raycastTarget = true;
            GameLoop.Instance.SelectCard(null);
            GameLoop.Instance.SelectTile(null);
            GameLoop.Instance.CardUsed -= OnCardGetsUsed;
        }

        private void OnCardGetsUsed(object sender, EventArgs e)
        {
            GameLoop.Instance.CardUsed -= OnCardGetsUsed;
            transform.GetComponentInParent<CardCommandProviderView>().CardPlayed(this);
            Destroy(gameObject);
        }
    }
}
