
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HandController : List<Card>
{
    public int HandSize = 8;
    public int AmountToDraw => HandSize - Count;
    public int SelectedCount => this.Where(x => x.IsSelected).Count();

    public Action onSelectedChanged;

    public void OnCardClickHandle(Card card)
    {
        if (SelectedCount < 5 && !card.IsSelected)
        {
            card.IsSelected = true;
            card.transform.DOLocalMoveY(100, 0.5f);
            onSelectedChanged?.Invoke();
        }
        else if (card.IsSelected)
        {
            card.IsSelected = false;
            card.transform.DOLocalMoveY(0, 0.5f);
            onSelectedChanged?.Invoke();
        }
    }
    public IEnumerable<Card> GetSelectedCards()
    {
        return this.Where(x => x.IsSelected);
    }
    
}
