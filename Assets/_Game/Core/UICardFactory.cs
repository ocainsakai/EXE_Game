using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class UICardFactory : MonoBehaviour
{
    [SerializeField] private CardEntry cardEntry;
    [SerializeField] private RectTransform container;
    private Dictionary<Card, CardEntry> cardEntries = new();

    public CardEntry GetOrCreateCard(Card card)
    {
        if (cardEntries.TryGetValue(card, out var entry) && entry != null)
        {
            return entry;
        }
       
        entry = Instantiate(cardEntry, container.position, Quaternion.identity);
        entry.transform.SetParent(container);
        entry.transform.localScale = Vector3.one;
        entry.SetCard(card);
        cardEntries.Add( card, entry);
        return entry;
        
    } 

    public void ReturnToPool(CardEntry entry)
    {
        entry.transform.DOLocalMoveX(1000f, 0.25f).OnComplete(() => entry.transform.SetParent(container));
        
    }
}
