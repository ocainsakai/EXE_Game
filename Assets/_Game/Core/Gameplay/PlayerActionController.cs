using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Game.Addons.Deck.Scripts;
using _Game.Addons.Deck.Scripts.Card;
using _Game.Addons.Deck.Scripts.CardCollection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace _Game.Core.Gameplay
{
    public class PlayerActionController : MonoBehaviour
    {
        [Header("System References"), SerializeField]
        
        private BattleSystem battleSystem;

        [FormerlySerializedAs("_deckManager"), SerializeField] 
        private DeckManager deckManager;

        [FormerlySerializedAs("_room"), SerializeField] 
        private Room room;

        [FormerlySerializedAs("_discardPile"), SerializeField] 
        private DiscardPile discardPile;

        [FormerlySerializedAs("_cardFactory"), SerializeField] 
        private UICardFactory cardFactory;

        [FormerlySerializedAs("_playerButton"), SerializeField] 
        private PlayerButton playerButton;

        [Header("Settings"), SerializeField]
        
        private bool isTestMode;

        public UnityEvent onStartTurn;
        [FormerlySerializedAs("OnEndTurn")] public UnityEvent<PlayerActionController> onEndTurn;

        private void Start()
        {
            if (isTestMode)
            {
                Debug.Log("[PlayerActionController] Test Mode enabled. Creating initial deck.");
                foreach (var card in deckManager.OriginCards)
                {
                    cardFactory.GetOrCreateCard(card);
                }
                ActivateRoom();
            }
        }

        private void ActivateRoom()
        {
            Debug.Log("[PlayerActionController] Activating Room. Creating runtime deck and starting player turn.");
            deckManager.CreateRuntimeDeck();
            PlayerStartTurn();
            room.UnselectAll();
        }

        private void PlayerDraw()
        {
            int cardNeed = room.RoomSize - room.Cards.Count;
            Debug.Log($"[PlayerActionController] Drawing {cardNeed} card(s).");
            for (int i = 0; i < cardNeed; i++)
            {
                var card = deckManager.DrawCard();
                if (card != null)
                {
                    room.Add(card);
                }
                else
                {
                    Debug.LogWarning("[PlayerActionController] DrawCard returned null. Deck might be empty.");
                    break;
                }
            }
            room.CurrentSort();
        }

        public void PlayerDiscard()
        {
            Debug.Log("[PlayerActionController] Attempting to discard selected cards.");
            playerButton.DisableAllActions();

            var cardsToDiscard = room.GetSelectCards;
            if (cardsToDiscard.Count == 0)
            {
                Debug.LogWarning("[PlayerActionController] Discard failed: No cards selected.");
                playerButton.EnableAllActions();
                return;
            }

            if (!battleSystem.CanDiscard())
            {
                Debug.LogWarning("[PlayerActionController] Discard failed: Cannot afford energy cost.");
                playerButton.EnableAllActions();
                return;
            }

            Debug.Log($"[PlayerActionController] Discarding {cardsToDiscard.Count} card(s).");
            battleSystem.UseEnergyDiscard();
            room.Discards();
            PlayerDraw();
            playerButton.EnableAllActions();
        }

        public void PlayerPlay()
        {
            Debug.Log("[PlayerActionController] Attempting to play selected cards.");
            var cardsToPlay = room.GetSelectCards;

            if (cardsToPlay.Count == 0)
            {
                Debug.LogWarning("[PlayerActionController] Play failed: No cards selected.");
                return;
            }

            if (!battleSystem.CanPlayHand(cardsToPlay))
            {
                Debug.LogWarning("[PlayerActionController] Play failed: Not enough energy or invalid hand.");
                return;
            }

            playerButton.DisableAllActions();
            Debug.Log($"[PlayerActionController] Playing hand with {cardsToPlay.Count} card(s): {string.Join(", ", cardsToPlay.Select(c => c.Name))}");
            StartCoroutine(PlayHand(cardsToPlay));
        }

        public void PlayerStartTurn()
        {
            Debug.Log("================= Player Start Turn =====================");
            battleSystem.RegenEnergy();
            room.Discards(); // Discard remaining cards from last turn
            PlayerDraw();
            playerButton.EnableAllActions();
            onStartTurn?.Invoke();
        }

        public void PlayerEndTurn()
        {
            Debug.Log("================= Player End Turn =======================");
            playerButton.DisableAllActions();
            room.UnselectAll();
            onEndTurn?.Invoke(this);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private IEnumerator PlayHand(List<CardRuntime> cards)
        {
            yield return ActivateCards(cards);
            playerButton.DisableAllActions();
            onEndTurn?.Invoke(this);
        }

        private IEnumerator ActivateCards(List<CardRuntime> cards)
        {
            // Create a copy to prevent issues if the original list is modified during iteration
            var cardsToActivate = new List<CardRuntime>(cards);
            foreach (var card in cardsToActivate)
            {
                Debug.Log($"[PlayerActionController] Activating card: {card.Name}");
                battleSystem.UseEnergyPlay();
                yield return card.Active();
            }
        }
    }
}
