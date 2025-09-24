using System;
using System.Collections.Generic;
using UnityEngine;

public class UICardManager : MonoBehaviour
{
    [SerializeField] private CardEntry cardEntry;

    private Dictionary<Card, CardEntry> cardEntries = new();
    public static UICardManager Singleton;
    private void Awake()
    {
        if (Singleton != null && Singleton != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Singleton = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public CardEntry Add(Card card, Transform transform = null)
    {
        var entry = Instantiate(cardEntry, transform ??= this.transform);
        entry.SetImage(card.CardData.Art);
        entry.OnCardClick += () => card.OnCardClickHandle();
        card.onSelectChange += () => entry.OnSelected(card.IsSelecting);
        cardEntries.Add(card,entry);
        return entry;
    }
    public CardEntry GetCard(Card card)
    {
        if (cardEntries.TryGetValue(card, out var entry))
        {
            return entry;
        }
        return null;
    }

    public List<CardEntry> GetCards(IEnumerable<Card> cards)
    {
        var result = new List<CardEntry>();
        foreach (var item in cards)
        {
            result.Add(GetCard(item));
        }
        return result;
    }
}
