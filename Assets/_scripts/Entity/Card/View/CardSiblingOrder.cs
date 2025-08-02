using System;
using UnityEngine;

public class CardSiblingOrder
{
    private CardContainer _container;
    public bool HigherOnRight { get; set; } = true;

    public CardSiblingOrder(CardContainer container)
    {
        _container = container;
    }

    public void UpdateOrder()
    {
        int childCount = _container.Cards.Count;
        if (childCount == 0) return;

        foreach (var item in _container.Cards)
        {
            int i = _container.Cards.IndexOf(item);
            item.transform.SetSiblingIndex(i);
        }
    }
}