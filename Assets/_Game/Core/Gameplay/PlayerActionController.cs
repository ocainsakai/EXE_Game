using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.Rendering.GPUSort;

public class PlayerActionController : MonoBehaviour
{
    [SerializeField] BattleSystem battleSystem;
    public DeckManager deck;
    public Room room;
    public DiscardPile discardPile;
    public UICardFactory cardFactory;

    public PlayerButton playerButton;
    private Coroutine playHandCoroutine;

    public bool IsTestMode;


    public UnityEvent OnStartTurn;
    public UnityEvent<PlayerActionController> OnEndTurn;






    private void Start()
    {
        if (IsTestMode)
        {
            foreach (var card in deck.OriginCards)
            {
                cardFactory.GetOrCreateCard(card);
            }
            ActiveRoom();
        }
    }


    public void ActiveRoom()
    {
        deck.CreateRuntimeDeck();
        PlayerStartTurn();
    }

    public void PlayerDraw()
    {
        int cardNeed = room.RoomSize - room.Cards.Count;
        for (int i = 0; i < cardNeed; i++)
        {
            var card = deck.DrawCard();
            room.Add(card);
        }
        room.CurrentSort();
    }
    public void PlayerDiscard()
    {
        var cards = room.GetSelectCards;
        //Debug.Log($"Player discards: ");
        if (!battleSystem.CanDiscard() || cards.Count == 0)
        {
            return;
        }
        battleSystem.UseEnergyDiscard();
        playerButton.DisableAllActions();
        DiscardAndEndTurn();
    }

    private void DiscardAndEndTurn()
    {
        room.Discards();
        PlayerEndTurn();
    }

    public void PlayerPlay()
    {
        var cards = room.GetSelectCards;
        if (!battleSystem.CanPlayHand(cards) || cards.Count == 0)
        {
            return;
        }
        playerButton.DisableAllActions();
        //Debug.Log("On play cards");
        StartCoroutine(PlayHand(cards));
    }
    public void PlayerStartTurn()
    {
        battleSystem.RegenEnergy();
        PlayerDraw();
        playerButton.EnableAllActions();
        OnStartTurn?.Invoke();
    }
    public void PlayerEndTurn()
    {
        playerButton.DisableAllActions();
        OnEndTurn?.Invoke(this);
    }
    public void StopPlayHand()
    {
        StopCoroutine(playHandCoroutine);
    }
    IEnumerator PlayHand(List<Card> cards)
    {
        yield return ActiveCards(cards);
        DiscardAndEndTurn();
    }
    IEnumerator ActiveCards(List<Card> cards)
    {
        foreach (var card in cards)
        {
            battleSystem.UseEnergyPlay();
            yield return card.Active();
        }

        // check condition
    }
}
