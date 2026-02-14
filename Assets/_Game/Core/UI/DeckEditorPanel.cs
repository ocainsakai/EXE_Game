using System.Collections.Generic;
using UnityEngine;
using _Game.Core.Data;
using _Game.Core.Systems;
using _Game.Core.DI;
using _Game.Core.UIElements;

namespace _Game.Core.UI
{
    public class DeckEditorPanel : BaseScreen
    {
        [Header("References")]
        [SerializeField] private Transform collectionContainer;
        [SerializeField] private Transform deckContainer;
        [SerializeField] private CardUI cardPrefab;

        private DeckManager _deckManager;

        protected override void Awake()
        {
            base.Awake();
            _deckManager = DIContainer.Global.Resolve<DeckManager>();
        }

        public override void Show()
        {
            base.Show();
            RefreshUI();
        }

        public void RefreshUI()
        {
            ClearContainers();
            
            // Populate Collection
            var ownedCards = _deckManager.GetOwnedCards();
            foreach (var card in ownedCards)
            {
                var cardUI = Instantiate(cardPrefab, collectionContainer);
                cardUI.Setup(card);
                // Add listener for adding to deck
            }

            // Populate Deck
            var currentDeck = _deckManager.GetCurrentDeck();
            foreach (var card in currentDeck)
            {
                var cardUI = Instantiate(cardPrefab, deckContainer);
                cardUI.Setup(card);
                // Add listener for removing from deck
            }
        }

        private void ClearContainers()
        {
            foreach (Transform child in collectionContainer) Destroy(child.gameObject);
            foreach (Transform child in deckContainer) Destroy(child.gameObject);
        }
    }
}
