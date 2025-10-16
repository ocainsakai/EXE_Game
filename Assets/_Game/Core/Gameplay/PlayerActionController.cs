using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PlayerActionController : MonoBehaviour
{
    [Header("System References")]
    [SerializeField] private BattleSystem _battleSystem;
    [SerializeField] private DeckManager _deckManager;
    [SerializeField] private Room _room;
    [SerializeField] private DiscardPile _discardPile;
    [SerializeField] private UICardFactory _cardFactory;
    [SerializeField] private PlayerButton _playerButton;

    [Header("Settings")]
    [SerializeField] private bool _isTestMode;

    [Header("Events")]
    public UnityEvent OnStartTurn;
    public UnityEvent<PlayerActionController> OnEndTurn;

    private void Start()
    {
        if (_isTestMode)
        {
            Debug.Log("[PlayerActionController] Test Mode enabled. Creating initial deck.");
            foreach (var card in _deckManager.OriginCards)
            {
                _cardFactory.GetOrCreateCard(card);
            }
            ActivateRoom();
        }
    }

    public void ActivateRoom()
    {
        Debug.Log("[PlayerActionController] Activating Room. Creating runtime deck and starting player turn.");
        _deckManager.CreateRuntimeDeck();
        PlayerStartTurn();
        _room.UnselectAll();
    }

    public void PlayerDraw()
    {
        int cardNeed = _room.RoomSize - _room.Cards.Count;
        Debug.Log($"[PlayerActionController] Drawing {cardNeed} card(s).");
        for (int i = 0; i < cardNeed; i++)
        {
            var card = _deckManager.DrawCard();
            if (card != null)
            {
                _room.Add(card);
            }
            else
            {
                Debug.LogWarning("[PlayerActionController] DrawCard returned null. Deck might be empty.");
                break;
            }
        }
        _room.CurrentSort();
    }

    public void PlayerDiscard()
    {
        Debug.Log("[PlayerActionController] Attempting to discard selected cards.");
        _playerButton.DisableAllActions();

        var cardsToDiscard = _room.GetSelectCards;
        if (cardsToDiscard.Count == 0)
        {
            Debug.LogWarning("[PlayerActionController] Discard failed: No cards selected.");
            _playerButton.EnableAllActions();
            return;
        }

        if (!_battleSystem.CanDiscard())
        {
            Debug.LogWarning("[PlayerActionController] Discard failed: Cannot afford energy cost.");
            _playerButton.EnableAllActions();
            return;
        }

        Debug.Log($"[PlayerActionController] Discarding {cardsToDiscard.Count} card(s).");
        _battleSystem.UseEnergyDiscard();
        _room.Discards();
        PlayerDraw();
        _playerButton.EnableAllActions();
    }

    public void PlayerPlay()
    {
        Debug.Log("[PlayerActionController] Attempting to play selected cards.");
        var cardsToPlay = _room.GetSelectCards;

        if (cardsToPlay.Count == 0)
        {
            Debug.LogWarning("[PlayerActionController] Play failed: No cards selected.");
            return;
        }

        if (!_battleSystem.CanPlayHand(cardsToPlay))
        {
            Debug.LogWarning("[PlayerActionController] Play failed: Not enough energy or invalid hand.");
            return;
        }

        _playerButton.DisableAllActions();
        Debug.Log($"[PlayerActionController] Playing hand with {cardsToPlay.Count} card(s): {string.Join(", ", cardsToPlay.Select(c => c.Name))}");
        StartCoroutine(PlayHand(cardsToPlay));
    }

    public void PlayerStartTurn()
    {
        Debug.Log("================= Player Start Turn =====================");
        _battleSystem.RegenEnergy();
        _room.Discards(); // Discard remaining cards from last turn
        PlayerDraw();
        _playerButton.EnableAllActions();
        OnStartTurn?.Invoke();
    }

    public void PlayerEndTurn()
    {
        Debug.Log("================= Player End Turn =======================");
        _playerButton.DisableAllActions();
        _room.UnselectAll();
        OnEndTurn?.Invoke(this);
    }

    private IEnumerator PlayHand(List<Card> cards)
    {
        yield return ActivateCards(cards);
        _playerButton.DisableAllActions();
        OnEndTurn?.Invoke(this);
    }

    private IEnumerator ActivateCards(List<Card> cards)
    {
        // Create a copy to prevent issues if the original list is modified during iteration
        var cardsToActivate = new List<Card>(cards);
        foreach (var card in cardsToActivate)
        {
            Debug.Log($"[PlayerActionController] Activating card: {card.Name}");
            _battleSystem.UseEnergyPlay();
            yield return card.Active();
        }
    }
}
