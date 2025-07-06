using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public DeckData deckData;
    public CardFactory cardFactory;
    public CardsMove cardsMove;
    public CardsContext cardsContext;
    public PokerManager pokerManager;
    public GamePhase gamePhase;
    public enum SortType
    {
        Rank,
        Suit
    }
    public SortType currentSortType;
    private List<Card> allCards = new List<Card>();
    public bool canUpdateUI = true;
    private void Awake()
    {
        cardFactory = GetComponent<CardFactory>();
        CardSelectHandler.OnClick.Subscribe(card => CardOnClickHandle(card));
        BlindCardContext();
        InitializeDeck();

    }

    private void BlindCardContext()
    {
        cardsContext.selected.ObserveCountChanged()
            .Subscribe(card =>
                    {   if (canUpdateUI)
                        pokerManager.UpdatePoker(cardsContext.selected);
                    });
        cardsContext.selected.ObserveAdd()
            .Subscribe(x => x.Value.Select());
        cardsContext.selected.ObserveRemove()
            .Subscribe(x => x.Value.Unselect());
    }

    private void Start()
    {
    }
    
    public void NextSortType()
    {
        SortType[] types = (SortType[])Enum.GetValues(typeof(SortType));
        int currentIndex = Array.IndexOf(types, currentSortType);
        int nextIndex = (currentIndex + 1) % types.Length;

        currentSortType = types[nextIndex];
        Sort();
    }
    public void Sort()
    {
        cardsContext.ResetHand();
        SortByType(currentSortType);
    }
    public void SortByType(SortType sortType)
    {
        switch (sortType)
        {
            case SortType.Rank:
                cardsContext.SortByRank();
                break;
            case SortType.Suit:
                cardsContext.SortBySuit();
                break;
        }
        cardsMove.MoveToHand(cardsContext.hand);
    }
    
    public void ResetBoard()
    {
        cardsMove.MoveToDiscard(cardsContext.played);
        gamePhase.currentPhase.Value = Phase.StartTurn;
    }
   
    private void InitializeDeck()
    {
        foreach (var item in deckData.startingCards)
        {
            var card = cardFactory.CreateCard(item, deckData.CardBack);
            card.gameObject.SetActive(true);
            allCards.Add(card);
        }
        cardsContext.deck = allCards.OrderBy(x => UnityEngine.Random.value).ToList();
    }
    private void CardOnClickHandle(Card card)
    {
        var select = card.GetComponent<CardSelectHandler>();
        if (select != null)
        {
            if (select.isSelecting)
            {
                cardsContext.selected.Add(card);
            }
            else
            {
                cardsContext.selected.Remove(card);
            }
        }
    }
    public void MoveToScore()
    {
        canUpdateUI = false;
        CardSelectHandler.IsBlockClick = true;
        var cards = cardsContext.TakeSelect();
        cardsContext.hand.RemoveAll(x => cards.Contains(x));
        //cardsContext.selected.Clear();

        cardsContext.played.AddRange(cards);
        cardsMove.MoveToScore(cards);
        cardsMove.MoveToHand(cardsContext.hand);
    }
    public void Discard()
    {
        var cards = cardsContext.TakeSelect();
        cardsContext.hand.RemoveAll(x => cards.Contains(x));

        cardsContext.selected.Clear();

        cardsMove.MoveToDiscard(cards);
    }
    public void DrawHand(int amout)
    {
        canUpdateUI = true;
        CardSelectHandler.IsBlockClick = false;

        if (amout > cardsContext.deck.Count)
        {
            return;
        }
        List<Card> cards = new List<Card>();
        cards = cardsContext.deck.GetRange(0, amout);
        cardsContext.deck.RemoveRange(0, amout);
        cardsContext.hand.AddRange(cards);
        Sort();
        cardsMove.MoveToHand(cardsContext.hand);
    }
}
