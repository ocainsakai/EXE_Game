
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Hand")]
public class HandController : Collection<Card>
{
    public int HandSize = 8;
    public int AmountToDraw => HandSize - Count;
    public int SelectedCount => this.Where(x => x .State.IsSelected).Count();

    public Action onSelectedChanged;

    private void OnEnable()
    {
        Card.onCardClicked += OnCardClickHandle;
    }

    private void OnDisable()
    {
        Card.onCardClicked -= OnCardClickHandle;
    }
    private void OnCardClickHandle(Card card)
    {
        if (SelectedCount < 5 && !card.State.IsSelected)
        {
            card.State.IsSelected = true;
            card.transform.DOLocalMoveY(100, 0.5f);
            onSelectedChanged?.Invoke();
        }
        else if (card.State.IsSelected)
        {
            card.State.IsSelected = false;
            card.transform.DOLocalMoveY(0, 0.5f);
            onSelectedChanged?.Invoke();
        }
    }
    public IEnumerable<Card> GetSelectedCards()
    {
        return this.Where(x => x.State.IsSelected);
    }
   
}
