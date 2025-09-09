using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class DeckManager : MonoBehaviour
{
    [SerializeField] CardView cardPref;
    [SerializeField] Transform CardTransform;
    [SerializeField] Transform deckTransform;
    [SerializeField] TextDisplay pokerText;
    [SerializeField] TextDisplay multText;
    private List<CardView> deck = new();
    private List<CardView> discardsPile = new();
    private List<CardView> cards = new();
    private List<CardView> hand = new();
    public Action OnCountChange;
    private int _currentSortType = 0;
    private void OnSelectHandle()
    {

      
    }
   
    private void ShuffleDeck()
    {
        deck = deck.OrderBy(x => UnityEngine.Random.value).ToList();
    }
    //public void Sort(bool toggle = false)
    //{
    //    //Debug.Log("soorrtttt");
    //    if (toggle) _currentSortType++;
    //    switch (_currentSortType % 2)
    //    {
    //        case 0:
    //            SortByRank();
    //            break;
    //        case 1:
    //            SortBySuit();
    //            break;
    //    }
    //}
    //private void SortByRank()
    //{
    //    var sorted = handController.OrderBy(x => x.Data.Rank).ThenBy(x => x.Data.Suit).ToList();
    //    handController.Clear();
    //    foreach (var card in sorted)
    //    {
    //        handController.Add(card);
    //    }
    //    //DeckState.Hand = handController.Select(x => x.ID).ToList();
    //    OnCountChange?.Invoke();
    //}
    //private void SortBySuit()
    //{
    //    var sorted = handController.OrderBy(x => x.Data.Suit).ThenBy(x => x.Data.Rank).ToList();
    //    handController.Clear();
    //    foreach (var card in sorted)
    //    {
    //        handController.Add(card);
    //    }
    //    //DeckState.Hand = handController.Select(x => x.ID).ToList();
    //    OnCountChange?.Invoke();
    //}
    //public async UniTask DrawCards()
    //{
    //    // update state
    //    int amount = handController.AmountToDraw;

    //    var cardsDrawn = new List<Card>();
    //    if (amount > deck.Count)
    //    {
    //        amount -= deck.Count;
    //        cardsDrawn.AddRange(deck);
    //        deck.Clear();
    //        await RestoreFormDiscards();
    //    }
    //    cardsDrawn.AddRange(deck.Take(amount));
    //    deck.RemoveRange(0, amount);


    //    // update ui
    //    foreach (var card in cardsDrawn)
    //    {
    //        handController.Add(card);
    //    }
    //    Sort();
    //}

    private async UniTask RestoreFormDiscards()
    {
        List<UniTask> tasks = new List<UniTask>();
        deck.AddRange(discardsPile);
        foreach (var card in discardsPile)
        {
            card.gameObject.SetActive(true);
            card.transform.SetParent(deckTransform);
            var task = card.transform.DOLocalMove(Vector3.zero, 0.5f).AsyncWaitForCompletion().AsUniTask();
            tasks.Add(task);
        }
        await UniTask.WhenAll(tasks);
        discardsPile.Clear();
        ShuffleDeck();
    }

    //public async UniTask Discards()
    //{
    //    List<UniTask> tasks = new();
    //    // update state
    //    var cardSelected = handController.GetSelectedCards().ToList();
    //    //var cardsID = cardSelected.Select(x => x.ID).ToList();
    //    discardsPile.AddRange(cardSelected);
    //    foreach (var item in cardSelected)
    //    {
    //        handController.Remove(item);
    //        item.transform.SetParent(CardTransform);
    //        var task = item.transform.DOLocalMove(Vector3.zero, 0.5f).AsyncWaitForCompletion().AsUniTask();
    //        //item.gameObject.SetActive(false);
    //        tasks.Add(task);
    //    }
    //    await UniTask.WhenAll(tasks);
    //    //await handDisplay.OnCountChangedHandle() ;

    //}
    //public void ClearState()
    //{
    //    deck.Clear();
    //    discardsPile.Clear();
    //    handController.Clear();
    //    cards.Clear();
    //    Card.onCardClicked -= handController.OnCardClickHandle;
    //}
}
