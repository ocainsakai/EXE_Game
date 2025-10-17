using DG.Tweening;
using System.Collections.Generic;
using _Game.Addons.Deck.Scripts;
using _Game.Addons.Deck.Scripts.Card;
using UnityEngine;

public class UICardFactory : MonoBehaviour
{
    [SerializeField] private CardEntry cardEntry;
    [SerializeField] private RectTransform container;
    private Dictionary<CardRuntime, CardEntry> cardEntries = new();

    public CardEntry GetOrCreateCard(CardRuntime cardRuntime)
    {
        if (cardEntries.TryGetValue(cardRuntime, out var entry) && entry != null)
        {
            return entry;
        }
       
        entry = Instantiate(cardEntry, container.position, Quaternion.identity);
        entry.transform.SetParent(container);
        entry.transform.localScale = Vector3.one;
        entry.SetCard(cardRuntime);
        cardEntries.Add( cardRuntime, entry);
        return entry;
        
    } 

    public void ReturnToPool(CardEntry entry)
    {
        entry.transform.DOLocalMoveX(1000f, 0.25f).OnComplete(() => entry.transform.SetParent(container));
        
    }
}
