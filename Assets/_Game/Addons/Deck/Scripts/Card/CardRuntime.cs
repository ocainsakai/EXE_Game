using System;
using System.Collections;
using CardSystem;
using UnityEngine;

namespace _Game.Addons.Deck.Scripts.Card
{
    public class CardRuntime
    {
        public static Func<CardRuntime, IEnumerator> OnActive;

        private readonly CardData _cardData;
        public CardData CardData => _cardData;
        public int CardDataID => _cardData.CardID;
        public SerializableGuid CardID;

        [Header("Card Identity")]
        public CardRank Rank => _cardData.Rank;
        public CardSuit Suit => _cardData.Suit;

        [Header("Card Info")]
        public int Cost => _cardData.Cost;
        public string Name => _cardData.Name;
        [TextArea] public string Description;
        public Sprite Art => _cardData.Art;

        public CardMask Mask => new CardMask(Rank, Suit);

        public Action SelectedChanged;

        private bool isSelecting;
        public bool IsSelected
        {
            get => isSelecting;
            set
            {
                isSelecting = value;
                SelectedChanged?.Invoke();
            }
        }


        public CardRuntime(CardData data)
        {
            _cardData = data;
            isSelecting = false;
            CardID = SerializableGuid.NewGuid();
        }

        public override bool Equals(object obj)
        {
            return obj is CardRuntime other && other.CardID == CardID;
        }

        public override int GetHashCode()
        {
            return CardID.GetHashCode();
        }

        public IEnumerator Active()
        {
            if (OnActive != null)
            {
                // Lấy toàn bộ delegate trong event (nếu có nhiều listener)
                foreach (var d in OnActive.GetInvocationList())
                {
                    var func = (Func<CardRuntime, IEnumerator>)d;
                    yield return func(this); // chạy lần lượt từng listener
                }
            }

            Debug.Log("playing..."+ Name);
            yield return new WaitForSeconds(1);
        }
    }
}
