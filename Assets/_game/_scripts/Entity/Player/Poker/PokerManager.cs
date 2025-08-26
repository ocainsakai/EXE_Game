using System.Collections;
using UnityEngine;

public class PokerManager : MonoBehaviour
{
    [SerializeField] private PokerDisplay pokerDisplay;
    [SerializeField] private PokerDatabase pokerDatabase;
    private PokerHandEvaluator pokerHandEvaluator = new PokerHandEvaluator();
    private PokerHandType handType;
    private float mult;
    public void OnSelectChangedHandle(int[] ranks, int[] suits)
    {
        var result = pokerHandEvaluator.EvaluateHand(ranks, suits);
        handType = result;
        mult = pokerDatabase.GetMultiplier(result);

        pokerDisplay.UpdatePokerType(result);
        pokerDisplay.UpdatePokerMult((int)mult);
    }
}
