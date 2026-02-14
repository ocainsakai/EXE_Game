using UnityEngine;
using UnityEngine.UI;
using TMPro;
using _Game.Core.Data;

namespace _Game.Core.UI
{
    public class CardUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private Image artworkImage;
        [SerializeField] private Image rarityFame;
        [SerializeField] private TextMeshProUGUI suitRankText;

        private CardData _currentCard;

        public void Setup(CardData data)
        {
            _currentCard = data;
            
            if (nameText != null) nameText.text = data.cardName;
            if (descriptionText != null) descriptionText.text = data.description;
            if (artworkImage != null) artworkImage.sprite = data.artwork;
            if (suitRankText != null) suitRankText.text = $"{data.rank} of {data.suit}";
            
            // Set colors based on rarity if needed
            UpdateRarityVisuals(data.rarity);
        }

        private void UpdateRarityVisuals(CardRarity rarity)
        {
            if (rarityFame == null) return;
            
            Color color = rarity switch
            {
                CardRarity.Common => Color.white,
                CardRarity.Uncommon => Color.green,
                CardRarity.Rare => Color.blue,
                CardRarity.Epic => new Color(0.5f, 0, 0.5f), // Purple
                CardRarity.Legendary => new Color(1f, 0.5f, 0), // Orange
                _ => Color.white
            };
            rarityFame.color = color;
        }

        public void OnCardClicked()
        {
            // Logic for when a card is selected in the UI
            Debug.Log($"Card Clicked: {_currentCard.cardName}");
        }
    }
}
