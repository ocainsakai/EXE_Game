using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class HandEvaluator 
{
    private PokerData HUD;

    private PokerHandEvaluator pokerHandEvaluator;

    public HandEvaluator(PokerData hUD)
    {
        HUD = hUD;
        pokerHandEvaluator = new PokerHandEvaluator();
    }

    public void UpdateHUD(IEnumerable<Card> selectedCards)
    {
        Debug.Log("update UI");
        var rankList = selectedCards.Select(x => x.Data.Rank).ToArray();
        var suitList = selectedCards.Select(x => x.Data.Suit).ToArray();
        var type = pokerHandEvaluator.EvaluateHand(rankList, suitList);

        HUD.PokerType.Value = type;
    }
}
