using DG.Tweening;
using UnityEngine;

public class DiscardPile : BaseCardPile
{
    [SerializeField] UICardManager _cardManager;
    [SerializeField] private Transform discard;
    public override void Add(Card card)
    {
        base.Add(card);
        var entry = _cardManager.GetCard(card);
        if (entry == null) return;
        entry.transform.SetParent(discard);
        entry.transform.DOMove(discard.position, 0.1f);
    }

}
