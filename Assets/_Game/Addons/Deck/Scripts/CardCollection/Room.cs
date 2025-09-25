using CardSystem.PokerSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Room : BaseCardPile
{
    [SerializeField] BaseCardPile discard;
    [SerializeField] BaseCardPile deck;
    [SerializeField] UIRoom _uIRoom;
    [SerializeField] UIPokerMult _uIPokerMult;

    public Action OnDiscardComplete;
    public int RoomSize = 8;

    private bool sortToggle = true;
    public override void Add(Card card)
    {
        base.Add(card);
        card.onSelectChange += SelectHandler;
        card.IsSelecting = false;
        
    }

    public List<Card> SetectedCards => cards.Where(x => x.IsSelecting).ToList();
    private void SelectHandler()
    {
        // update can select
        Card.CanSelect = cards.Where(x => x.IsSelecting).Count() < 5;
        // update poker hand
        var pokerHand = PokerEvaluator.Evaluate(SetectedCards.Select(x => x.CardData.Mask).ToList());
        _uIPokerMult.SetPokerMult(pokerHand.HandType);
    }
    public void OnEnable()
    {
        _uIRoom?.gameObject.SetActive(true);
        Draw();
    }
    public void Draw()
    {
        // logic
        if (RoomSize > deck.Count)
        {
            discard.MoveAllTo(deck);
        }
        var cards = this.DrawFrom(deck, RoomSize - Cards.Count);

        //condition
        SelectHandler();

        //ui
        _uIRoom.AddRange(cards);
        Sort(sortToggle);
        _uIRoom.UpdateIndex(Cards);
       
       
    }
    public void OnDisable()
    {
        this.MoveAllTo(deck);
        if (_uIRoom == null) return;
        _uIRoom.gameObject.SetActive(false);

    }
    public void Discards()
    {
        var cardsToDiscard = Cards.Where(x => x.IsSelecting).ToList();
        if (cardsToDiscard.Count == 0) return;
        foreach (var card in cardsToDiscard)
        {
            this.MoveCardTo(discard, card);
            _uIRoom.Remove(card);
        }
        //OnDiscardComplete?.Invoke();
        Draw();
    }
    public void Sort()
    {
        sortToggle = !sortToggle;
        Sort(sortToggle);
        _uIRoom.UpdateIndex(cards);

    }
    private void Sort(bool isRank)
    {
        if (isRank)
        {
            SortBySuit();
        }
        else
        {
            SortByRank();
        }
    }
    public void SortByRank()
    {
        SortCards((a, b) =>
        {
            int rankCompare = a.CardData.Rank.CompareTo(b.CardData.Rank);
            return rankCompare != 0 ? rankCompare : a.CardData.Rank.CompareTo(b.CardData.Rank);
        });
    }
    public void SortBySuit()
    {
        SortCards((a, b) =>
        {
            int rankCompare = a.CardData.Suit.CompareTo(b.CardData.Suit);
            return rankCompare != 0 ? rankCompare : a.CardData.Suit.CompareTo(b.CardData.Suit);
        });

    }
}
