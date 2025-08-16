using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;

[Serializable]
public class CardList : List<Card>
{
    public void Add(Card card, Action onSelect)
    {
        Add(card);
        card.selecter.isSelected.Subscribe(_ => onSelect?.Invoke());
    }
    public void Shuffle()
    {
        for (int i = Count - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            (this[i], this[j]) = (this[j], this[i]); // swap
        }
    }
    public void SortByRank()
    {
        this.Sort((a, b) =>
        {
            int rankCompare = a.Data.Rank.CompareTo(b.Data.Rank);
            if (rankCompare != 0)
                return rankCompare;

            return a.Data.Suit.CompareTo(b.Data.Suit);
        });

        this.Reverse();
    }
    public void SortBySuit()
    {
        this.Sort((a, b) =>
        {
            int rankCompare = a.Data.Suit.CompareTo(b.Data.Suit);
            if (rankCompare != 0)
                return rankCompare;

            return a.Data.Rank.CompareTo(b.Data.Rank);
        });

        this.Reverse();
    }
    public List<Card> TakeSelected()
    {
        return this.Where(x => x.selecter.IsSelected).ToList();

    }
    public void ClearSelected()
    {
        RemoveAll(x => x.selecter.IsSelected);
    }
}
