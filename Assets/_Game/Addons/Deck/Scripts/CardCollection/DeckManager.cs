using System.Collections.Generic;
using System.Linq;
using _Game.Addons.Deck.Scripts;
using _Game.Addons.Deck.Scripts.Card;
using _Game.Core;
using CardSystem;
using Unity.VisualScripting;
using UnityEngine;

namespace _Game.Addons.Deck.Scripts.CardCollection
{
    public class DeckManager : MonoBehaviour
    {
        private List<Card.CardRuntime> originCards=new();
        private List<Card.CardRuntime> cards=new();
        public IReadOnlyCollection<Card.CardRuntime> OriginCards => originCards;
        public IReadOnlyCollection<Card.CardRuntime> Cards => cards;

        public Sprite DeckCover { get; private set; }
        public int CardCount => cards.Count;  

        public bool IsTestMode;

        public DeckData testDeck;
        public bool DestroyCard(SerializableGuid cardID)
        {
            if (originCards.Select(x => x.CardID).Contains(cardID))
            {
                var item = originCards.FirstOrDefault(x => x.CardID == cardID);
                originCards.Remove(item);
                return true;
            }
            return false;
        }

        public Card.CardRuntime CreateCard(CardData data)
        {
            Card.CardRuntime cardRuntime = new(data);
            originCards.Add(cardRuntime);
            return cardRuntime;
        }

        public IEnumerable<Card.CardRuntime> CreateCards(IEnumerable<CardData> cardsData)
        {
            foreach (var cardData in cardsData)
            {
                yield return CreateCard(cardData);
            }
        }

        public Card.CardRuntime DrawCard()
        {
            var card = cards.FirstOrDefault();
            cards.Remove(card);
            return card;
        }
        public void ShuffleDeck()
        {
            for (int i = cards.Count - 1; i > 0; i--)
            {
                int randomIndex = Random.Range(0, i + 1);
                (cards[i], cards[randomIndex]) = (cards[randomIndex], cards[i]);
            }
        }

        public void Clear()
        {
            originCards.Clear();
            cards.Clear();
        }

        public void AddCardsAndShuffle(List<CardRuntime> startingCards)
        {
            originCards.AddRange(startingCards);    
            cards.AddRange(startingCards);
            ShuffleDeck();
        }

        public CardRuntime Draw()
        {
            var card = cards.FirstOrDefault();
            cards.Remove(card);
            return card;
        }
    }
}
