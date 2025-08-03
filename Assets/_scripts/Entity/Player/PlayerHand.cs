using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    [SerializeField] PlayerStateMachine playerStateMachine;
    [SerializeField] CardList hand;
    [SerializeField] PokerData HUD;
    private PokerHandEvaluator pokerHandEvaluator = new PokerHandEvaluator();
    private int currentSortType = 1;
    public int HandSize = 8;
    private void Awake()
    {
        hand.cards.Clear();
    }
    public List<Card> TakeSelected()
    {
        return hand.cards.Where(x => x.selecter.IsSelected).ToList();
        
    }
    public void ClearSelected()
    {
        hand.cards.RemoveAll(x => x.selecter.IsSelected);
    }
    public void DrawCard(CardList deck)
    {
        int amount = HandSize - hand.cards.Count;
        if (amount <= 0 || deck.cards.Count < amount) return;
        for (int i = 0; i < amount; i++)
        {
            if (deck.cards.Count == 0) break;
            Card card = deck.cards[0];
            hand.cards.Add(card);
            SetupCard(card);
            deck.cards.RemoveAt(0);
        }

        Sort(currentSortType);
    }

    private void SetupCard(Card card)
    {
        card.transform.SetParent(transform);
        card.gameObject.SetActive(true);
        card.TurnOnInput();
        card.selecter.isSelected.Subscribe(CheckSelect).AddTo(this);
    }

    private void CheckSelect(bool obj)
    {
        //Debug.Log(obj);
        var selectedCards = TakeSelected();
        if (selectedCards.Count >= 5)
        {
            hand.cards.ForEach(x => x.selecter.CanSelect = false);
        }
        else
        {
            hand.cards.ForEach(x => x.selecter.CanSelect = true);
        }
        UpdateHUD(selectedCards);
    }
    private void UpdateHUD(IEnumerable<Card> selectedCards)
    {

        var rankList = selectedCards.Select(x => x.Data.Rank).ToArray();
        var suitList = selectedCards.Select(x => x.Data.Suit).ToArray();
        var type = pokerHandEvaluator.EvaluateHand(rankList, suitList);

        HUD.PokerType.Value = type;
    }
    public void Discard(CardList discardPile)
    {
        CardFactory.Instance.ReturnCards(TakeSelected());
        ClearSelected();
        UpdateHUD(TakeSelected());
        
    }
    public void Sort()
    {
        currentSortType = -currentSortType;
        Sort(currentSortType);
    }
    private void Sort(int sortType)
    {
        if (sortType > 0)
        {
            hand.cards = hand.cards.OrderBy(x => x.Data.Rank).ThenBy(x => x.Data.Suit).ToList();
        }
        else
        {
            hand.cards = hand.cards.OrderBy(x => x.Data.Suit).ThenBy(x => x.Data.Rank).ToList();
        }
        hand.cards.ForEach(x => x.transform.SetSiblingIndex(hand.cards.Count - 1));
    }
}
