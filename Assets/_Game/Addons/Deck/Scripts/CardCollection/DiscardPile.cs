using DG.Tweening;
using UnityEngine;

public class DiscardPile : BaseCardPile
{
    [SerializeField] UICardFactory _cardManager;
    [SerializeField] private Transform discard;
    public override void Add(Card card)
    {
        base.Add(card);
        var entry = _cardManager.GetOrCreateCard(card);
        if (entry == null) return;
        entry.transform.SetParent(discard);
        entry.transform.DOMove(discard.position, 0.1f);
    }

}
