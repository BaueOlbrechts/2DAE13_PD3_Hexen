using GameSystem.Models;
using MoveSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameSystem.Views
{
    public class CardCommandProviderView : MonoBehaviour
    {
        [SerializeField]
        private int _deckSize = 9;

        [SerializeField]
        private int _handSize = 5;

        [SerializeField]
        private CardCommandView[] _cardPrefabs = new CardCommandView[4];

        private List<CardCommandView> _cardsInHand = new List<CardCommandView>();
        private List<CardCommandView> _deck = new List<CardCommandView>();

        public CardManager<BoardPiece> CardManager = null;

        private void Start()
        {
            GenerateDeck();
            DrawStartingCards();
        }

        private void GenerateDeck()
        {
            int cardsLeftToAdd = _deckSize;
            int commonDivider = _deckSize / _cardPrefabs.Length;
            cardsLeftToAdd -= commonDivider * _cardPrefabs.Length;

            //Average distribution of cards based on decksize
            for (int i = 0; i < commonDivider; i++)
            {
                foreach (var cardCommandView in _cardPrefabs)
                {
                    var card = Instantiate(cardCommandView, transform);
                    card.Command = CardManager.GetCardCommand(card.CardCommandName);
                    _deck.Add(card);
                }
            }

            //Randomly assigned cards based on leftoves decksize
            for (int i = 0; i < cardsLeftToAdd; i++)
            {
                var idx = UnityEngine.Random.Range(0, _cardPrefabs.Length);
                var prefab = _cardPrefabs[idx];
                var card = Instantiate(prefab, transform);
                card.Command = CardManager.GetCardCommand(card.CardCommandName);
                _deck.Add(card);
            }

            foreach (var card in _deck)
            {
                card.gameObject.SetActive(false);
            }
        }

        private void DrawStartingCards()
        {
            for (int i = 0; i < _handSize; i++)
            {
                var idx = UnityEngine.Random.Range(0, _deck.Count);
                var card = _deck[idx];

                if (card.gameObject.activeInHierarchy)
                {
                    i--;
                    continue;
                }

                card.gameObject.SetActive(true);

                _cardsInHand.Add(card);
                _deck.Remove(card);
            }
        }

        private void DrawCard()
        {
            if (_deck.Count > 0)
            {
                var idx = UnityEngine.Random.Range(0, _deck.Count);
                var card = _deck[idx];

                if (card.gameObject.activeInHierarchy)
                {
                    DrawCard();
                    return;
                }

                card.gameObject.SetActive(true);

                _cardsInHand.Add(card);
                _deck.Remove(card);
            }
        }

        public void CardPlayed(CardCommandView playedCard)
        {
            _cardsInHand.Remove(playedCard);
            DrawCard();
        }
    }
}
