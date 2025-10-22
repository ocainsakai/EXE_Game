using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace _Game.Core.Gameplay
{
    public class PlayerActionController : MonoBehaviour
    {
        [Header("System References")]
        
        [SerializeField] private CardManager cardManager;
        [SerializeField] private BattleMediator battleMediator;
        [SerializeField] private ScoreManager scoreManager;
        [SerializeField] private PlayerButton playerButton;
        [SerializeField] private GameObject ui;
        [Header("Settings"), SerializeField]
        
        private bool isTestMode;

        public UnityEvent onStartTurn;
        public UnityEvent<PlayerActionController> onEndTurn;
        
        public void PlayerDiscard()
        {
            Debug.Log("[PlayerActionController] Attempting to discard selected cards.");
            playerButton.DisableAllActions();
            if (!battleMediator.CanAffordDiscard())
            {
                Debug.LogWarning("[PlayerActionController] Discard failed: Cannot afford energy cost.");
                playerButton.EnableAllActions();
                return;
            }

            if (!cardManager.DiscardSelectedCards())
            {
                playerButton.EnableAllActions();
                return;
            }
            battleMediator.UseEnergyDiscard();
            cardManager.DrawHand();
            playerButton.EnableAllActions();
        }

        public void PlayerPlay()
        {
            Debug.Log("[PlayerActionController] Attempting to play selected cards.");
            var cardsToPlay = cardManager.SelectedCards.Count;

            if (cardsToPlay == 0)
            {
                Debug.LogWarning("[PlayerActionController] Play failed: No cards selected.");
                return;
            }

            if (!battleMediator.CanAffordPlayHand(cardsToPlay))
            {
                Debug.LogWarning("[PlayerActionController] Play failed: Not enough energy or invalid hand.");
                return;
            }

            playerButton.DisableAllActions();
            StartCoroutine(PlayHand());
        }

        public void StartTurn()
        {
            Debug.Log("================= Player Start Turn =====================");
            battleMediator.RegenEnergy();
            cardManager.DrawHand();
            playerButton.EnableAllActions();
            onStartTurn?.Invoke();
        }

        public void PlayerEndTurn()
        {
            playerButton.DisableAllActions();
            onEndTurn?.Invoke(this);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private IEnumerator PlayHand()
        {
            yield return scoreManager.ActivateCards(cardManager.SelectedCards);
            cardManager.DiscardSelectedCards();
            PlayerEndTurn();
        }
        public void Active()
        {
            ui.gameObject.SetActive(true);
            StartTurn();
        }
    }
}
