using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class DeckManager : MonoBehaviour
{
    [SerializeField] CardFactory cardFactory;
    [SerializeField] CardsDisplay handDisplay;
    [SerializeField] HandController handController;
    [SerializeField] PokerManager pokerManager;

    public DeckState DeckState;
    private Dictionary<SerializableGuid, Card> cards = new Dictionary<SerializableGuid, Card>();

    private int _currentSortType = 0;
    private void OnEnable()
    {
        handController.onSelectedChanged += OnSelectHandle;
    }
    private void OnDisable()
    {
        handController.onSelectedChanged -= OnSelectHandle;
    }

    private void OnSelectHandle()
    {
        var ranks = handController.GetSelectedCards().Select(x => x.State.CardSDData.Rank).ToArray();
        var suits = handController.GetSelectedCards().Select(x => x.State.CardSDData.Suit).ToArray();
        pokerManager.OnSelectChangedHandle(ranks, suits);
    }

    public void BuidDeck(IEnumerable<CardSDData> datas)
    {
        var cards = cardFactory.CreateCards(datas);
        foreach (var card in cards) {
            this.cards.Add(card.State.CardStateId, card);
        }
        DeckState.DeckOrder = cards.Select(x => x.State.CardStateId).ToList();
        ShuffleDeck();
    }
    public Card GetCard(SerializableGuid cardId)
    {
        return cards[cardId];
    }
    public IEnumerable<Card> GetCards(IEnumerable<SerializableGuid> cardId)
    {
        var cardList = new List<Card>();
        foreach (var card in cardId) {
            cardList.Add(GetCard(card));
        }
        return cardList;
    }
    private void ShuffleDeck()
    {
        DeckState.DeckOrder = DeckState.DeckOrder.OrderBy(x => UnityEngine.Random.value).ToList();
    }
    public async UniTask Sort(bool toggle = false)
    {
        //Debug.Log("soorrtttt");
        if (toggle) _currentSortType++;
        switch (_currentSortType % 2)
        {
            case 0:
                await SortByRank();
                break;
            case 1:
                await SortBySuit();
                break;
        }
    }
    private async UniTask SortByRank()
    {
        var sorted = handController.OrderBy(x => x.State.CardSDData.Rank).ThenBy(x => x.State.CardSDData.Suit).ToList();
        handController.Clear();
        foreach (var card in sorted)
        {
            handController.Add(card);
        }
        DeckState.Hand = handController.Select(x => x.State.CardStateId).ToList();
        await handDisplay.OnCountChangedHandle();
    }
    private async UniTask SortBySuit()
    {
        var sorted = handController.OrderBy(x => x.State.CardSDData.Suit).ThenBy(x => x.State.CardSDData.Rank).ToList();
        handController.Clear();
        foreach (var card in sorted)
        {
            handController.Add(card);
        }
        DeckState.Hand = handController.Select(x => x.State.CardStateId).ToList();
        await handDisplay.OnCountChangedHandle();
    }
    public async UniTask DrawCards()
    {
        // update state
        int amount = handController.AmountToDraw;

        if (amount > DeckState.DeckOrder.Count)
        {

            DeckState.DeckOrder.AddRange(DeckState.DiscardPile);
            DeckState.DiscardPile.Clear();
            ShuffleDeck();
        }

        var cardsDrawn = DeckState.DeckOrder.Take(amount).ToList();
        DeckState.Hand.AddRange(cardsDrawn);
        DeckState.DeckOrder.RemoveRange(0, amount);


        // update ui
        foreach (var card in cardsDrawn)
        {
            handController.Add(GetCard(card));
        }
        await Sort();
    }

    public async UniTask Discards()
    {

        // update state
        var cardSelected = handController.GetSelectedCards().ToList();
        Debug.Log(cardSelected.Count());
        var cardsID = cardSelected.Select(x => x.State.CardStateId).ToList();
        DeckState.DiscardPile.AddRange(cardsID);
        DeckState.Hand.RemoveAll(card => cardsID.Contains(card));

        foreach (var item in cardsID)
        {
            var card = GetCard(item);
            handController.Remove(card);
            cardFactory.ReturnCard(card);
        }
        await handDisplay.OnCountChangedHandle() ;

    }
}
