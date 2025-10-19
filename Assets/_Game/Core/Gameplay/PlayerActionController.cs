using System;
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
        [Header("System References")]
        [SerializeField]
        private CardManager cardManager;
        [SerializeField]
        private BattleSystem battleSystem;

        [SerializeField] 
        private PlayerButton playerButton;

        [SerializeField] private GameObject ui;
        [Header("Settings"), SerializeField]
        
        private bool isTestMode;

        public UnityEvent onStartTurn;
        public UnityEvent<PlayerActionController> onEndTurn;
        
        public void PlayerDiscard()
        {
            Debug.Log("[PlayerActionController] Attempting to discard selected cards.");
            playerButton.DisableAllActions();
            if (!battleSystem.CanDiscard())
            {
                Debug.LogWarning("[PlayerActionController] Discard failed: Cannot afford energy cost.");
                playerButton.EnableAllActions();
                return;
            }
            battleSystem.UseEnergyDiscard();
            cardManager.DiscardSelectedCards();
            cardManager.DrawHand();
            playerButton.EnableAllActions();
        }

        public void PlayerPlay()
        {
            Debug.Log("[PlayerActionController] Attempting to play selected cards.");
            var cardsToPlay = cardManager.SelectedCards;

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

        public void StartTurn()
        {
            Debug.Log("================= Player Start Turn =====================");
            battleSystem.RegenEnergy();
            cardManager.DiscardSelectedCards(); // Discard remaining cards from last turn
            cardManager.DrawHand();
            playerButton.EnableAllActions();
            onStartTurn?.Invoke();
        }

        public void PlayerEndTurn()
        {
            Debug.Log("================= Player End Turn =======================");
            playerButton.DisableAllActions();
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

        public void Active()
        {
            ui.gameObject.SetActive(true);
            StartTurn();
        }
    }
}
