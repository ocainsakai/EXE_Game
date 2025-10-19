using DG.Tweening;
using System.Collections.Generic;
using _Game.Addons.Deck.Scripts;
using _Game.Addons.Deck.Scripts.Card;
using UnityEngine;
using UnityUtils;

public class UICardFactory : MonoBehaviour
{
    [SerializeField] private CardEntry cardEntry;
    [SerializeField] private RectTransform container;
    private Dictionary<CardRuntime, CardEntry> cardEntries = new();

    public List<CardEntry> GetOrCreateCardEntries(List<CardRuntime> cards)
    {
        var list = new List<CardEntry>();
        foreach (var card in cards)
        {
            list.Add(GetOrCreateCard(card));
        }
        return list;    
    }
    public CardEntry GetOrCreateCard(CardRuntime cardRuntime)
    {
        if (cardEntries.TryGetValue(cardRuntime, out var entry) && entry != null)
        {
            return entry;
        }
       
        entry = Instantiate(cardEntry, container.position, Quaternion.identity);
        entry.transform.SetParent(container);
        entry.transform.localScale = Vector3.one;
        entry.Setup(cardRuntime);
        cardEntries.Add( cardRuntime, entry);
        return entry;
    }
    public void ReturnToPool(CardEntry entry)
    {
        entry.transform.DOKill(); // 🔒 Dừng tween cũ
        entry.transform.SetParent(container);
        entry.transform.DOLocalMoveX(1000f, 0.25f);
    }

    public void DestroyAll()
    {
        foreach (var entry in cardEntries.Values)
        {
            if (entry != null)
            {
                entry.transform.DOKill(); // 🔒 Hủy tween trước khi xóa
                Destroy(entry.gameObject);
            }
        }

        cardEntries.Clear();
    }

    public void ReturnToPool(CardRuntime cardRuntime)
    {
        var entry = GetOrCreateCard(cardRuntime);
        ReturnToPool(entry);
    }
}
