using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public DeckData deckData;
    public CardFactory cardFactory;
    public CardsMove moveCards;

    public CardsContext cardsContext;

    public enum SortType
    {
        Rank,
        Suit
    }
    public SortType currentSortType;
    private List<Card> allCards = new List<Card>();

    private void Awake()
    {
        cardFactory = GetComponent<CardFactory>();
        CardSelectHandler.OnClick.Subscribe(card => CardOnClickHandle(card));
    }
    #region
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
        moveCards.MoveToHand(cardsContext.hand);
    }
    #endregion
    private void Start()
    {
        InitializeDeck();
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

    public void Discard()
    {
        List<Card> cards = new List<Card>();
        cards = cardsContext.selected.ToList();
        cardsContext.hand.RemoveAll(x => cards.Contains(x));
        cardsContext.selected.Clear();
        moveCards.MoveToDiscard(cards);
    }
    public void DrawHand(int amout)
    {
        if (amout > cardsContext.deck.Count)
        {
            return;
        }
        List<Card> cards = new List<Card>();
        cards = cardsContext.deck.GetRange(0, amout);
        cardsContext.deck.RemoveRange(0, amout);
        cardsContext.hand.AddRange(cards);
        Sort();
        moveCards.MoveToHand(cardsContext.hand);
    }
}
