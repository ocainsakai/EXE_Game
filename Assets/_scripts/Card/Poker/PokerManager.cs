using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PokerManager : MonoBehaviour
{
    public PokerTypeData pokerData;
    public List<Card> matchedCards = new();
    public TextMeshPro pokerText;

    private PokerTypeEvaluator pokerTypeEvaluator;
    private void Awake()
    {
        pokerTypeEvaluator = GetComponent<PokerTypeEvaluator>();
    }
    public void UpdatePoker(IEnumerable<Card> cards)
    {
        var result = pokerTypeEvaluator.Evaluate(cards);
        pokerData = result.data;
        matchedCards = result.cards;
        pokerText.text = $"{pokerData.Type}\n" +
                            $"Mult: {pokerData.BaseMult}";
    }
}
