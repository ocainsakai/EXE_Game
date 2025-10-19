// CardRuntime.cs - Refactored
using System;
using System.Collections;
using CardSystem;
using UnityEngine;

namespace _Game.Addons.Deck.Scripts.Card
{
    public class CardRuntime
    {
        // THAY ĐỔI 1: Bỏ 'static' và dùng 'event' để an toàn hơn
        public event Func<CardRuntime, IEnumerator> OnActive;

        private readonly CardData _cardData;
        public CardData CardData => _cardData;
        public SerializableGuid CardID { get; }

        // --- Các thuộc tính chỉ đọc từ CardData ---
        public CardRank Rank => _cardData.Rank;
        public CardSuit Suit => _cardData.Suit;
        public int Cost => _cardData.Cost;
        public string Name => _cardData.Name;
        public Sprite Art => _cardData.Art;
        
        // THAY ĐỔI 2: Tính toán Mask một lần duy nhất để tối ưu
        public CardMask Mask { get; }

        // --- Trạng thái Runtime ---
        public event Action SelectedChanged;
        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected == value) return; // Không làm gì nếu trạng thái không đổi
                _isSelected = value;
                SelectedChanged?.Invoke();
            }
        }

        public CardRuntime(CardData data)
        {
            _cardData = data;
            CardID = SerializableGuid.NewGuid();
            Mask = new CardMask(Rank, Suit); // Tính toán ở đây
            _isSelected = false;
        }

        public override bool Equals(object obj) => obj is CardRuntime other && other.CardID == CardID;
        public override int GetHashCode() => CardID.GetHashCode();

        public IEnumerator Active()
        {
            if (OnActive != null)
            {
                foreach (var handler in OnActive.GetInvocationList())
                {
                    yield return (IEnumerator)handler.DynamicInvoke(this);
                }
            }
            Debug.Log("Playing... " + Name);
        }
    }
}