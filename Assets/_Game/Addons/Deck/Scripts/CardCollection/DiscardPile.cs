using _Game.Addons.Deck.Scripts;
using _Game.Addons.Deck.Scripts.Card;
using DG.Tweening;
using UnityEngine;

public class DiscardPile : BaseCardPile
{
    [SerializeField] UICardFactory _cardManager;
    [SerializeField] private Transform discard;
    public override void Add(CardRuntime cardRuntime)
    {
        base.Add(cardRuntime);
        var entry = _cardManager.GetOrCreateCard(cardRuntime);
        if (entry == null) return;
        entry.transform.SetParent(discard);
        entry.transform.DOMove(discard.position, 0.1f);
    }

}
