using CardSystem;
using CardSystem.PokerSystem;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
/// <summary>
/// Tính damage từ hand đã chọn
/// </summary>
public class DamageCalculator
{
    private readonly MultTable _multTable;

    public DamageCalculator(MultTable multTable)
    {
        _multTable = multTable;
    }

    /// <summary>
    /// Tính damage tổng
    /// </summary>
    public int Calculate(List<Card> selectedCards, out PokerHandResult handResult)
    {
        // Validate input
        if (selectedCards == null || selectedCards.Count == 0)
        {
            handResult = new PokerHandResult
            {
                HandType = PokerHandType.None,
                BestCards = new List<CardMask>()
            };
            return 0;
        }

        // Convert Card → CardMask
        var cardMasks = selectedCards.Select(c => c.Mask).ToList();

        // Evaluate poker hand
        handResult = PokerEvaluator.Evaluate(cardMasks);

        // Tính base damage từ các lá bài
        int baseDamage = CalculateBaseDamage(selectedCards);

        // Lấy multiplier từ poker hand
        float multiplier = _multTable.GetMult(handResult.HandType);

        // Damage = (Base × Multiplier) + Chips
        int totalDamage = Mathf.RoundToInt(baseDamage * multiplier);

        Debug.Log($"[DamageCalc] Hand: {handResult.HandType} | Base: {baseDamage} | Mult: {multiplier}x | Total: {totalDamage}");

        return totalDamage;
    }

    /// <summary>
    /// Tính base damage từ tổng điểm các lá bài
    /// </summary>
    private int CalculateBaseDamage(List<Card> cards)
    {
        int total = 0;
        foreach (var card in cards)
        {
            total += GetCardChipValue(card);
        }
        return total;
    }

    /// <summary>
    /// Lấy giá trị chip của 1 lá bài
    /// Balatro: J/Q/K = 10, Ace = 11, còn lại = rank value
    /// </summary>
    private int GetCardChipValue(Card card)
    {
        switch (card.Mask.ERank)
        {
            case CardRank.Ace:
                return 11;
            case CardRank.King:
            case CardRank.Queen:
            case CardRank.Jack:
                return 10;
            default:
                return (int)card.Mask.ERank;
        }
    }
}