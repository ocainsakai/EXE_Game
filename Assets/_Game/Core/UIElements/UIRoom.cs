using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIRoom : MonoBehaviour
{
    [SerializeField] private UICardPool cardFactory;
    private List<CardEntry> cardEntries = new List<CardEntry>();

    public void UpdateInteract(bool canSelect)
    {
        Debug.Log("UI update" + canSelect);
        foreach (CardEntry entry in cardEntries)
        {
            entry.SetButton(canSelect);
        }
    }
    public void UpdateCards(List<Card> newCards)
    {
        Debug.Log($"UpdateCards: {newCards.Count}");

        // map nhanh card → entry
        var entryMap = cardEntries.ToDictionary(e => e.CardID, e => e);
        var newIds = newCards.Select(c => c.CardID).ToList();

        // nhóm discard
        var discard = cardEntries
            .Where(e => !newIds.Contains(e.CardID))
            .ToList();

        // nhóm add
        var add = newCards
         .Where(c => !entryMap.ContainsKey(c.CardID))
         .Select(c => cardFactory.GetOrCreateCard(c))
         .ToList();

        // nhóm hold
        var hold = cardEntries
            .Where(e => newIds.Contains(e.CardID))
            .ToList();

        // chia hold thành unchanged / changed
        var unchanged = new List<CardEntry>();
        var changed = new List<CardEntry>();

        for (int i = 0; i < newIds.Count; i++)
        {
            var id = newIds[i];
            if (entryMap.TryGetValue(id, out var entry))
            {
                int oldIndex = cardEntries.IndexOf(entry);
                if (oldIndex == i)
                    unchanged.Add(entry);
                else
                    changed.Add(entry);
            }
        }

        // --- xử lý ---
        // 1. discard
        foreach (var e in discard)
        {
            DiscardFromRoom(e);
        }

        // 2. add
        foreach(var e in add)
        {
            AddCardToRoom(e);
        }

        // 3. re-order theo newCards
        cardEntries = newIds.Select(id =>
        entryMap.ContainsKey(id) ? entryMap[id] : add.First(a => a.CardID == id) ).ToList();

        // 4. reposition (UI layout)
        RepositionCards();

        // Debug log
        Debug.Log($"Discard: {discard.Count}, Add: {add.Count}, Hold: {hold.Count} (unchanged {unchanged.Count}, changed {changed.Count})");
    }

    public void AddCardToRoom(Card card)
    {
        var entry = cardFactory.GetOrCreateCard(card);
        AddCardToRoom(entry);
    }

    public void AddCardToRoom(CardEntry entry)
    {
        if (entry == null || cardEntries.Contains(entry)) return;
        entry.transform.SetParent(transform);
        cardEntries.Add(entry);
        entry.transform
            .DOMove(transform.position, 0.1f)
            .SetEase(Ease.OutQuad);
    }
    public void DiscardFromRoom(CardEntry card)
    {
        //var entry = cardEntries.FirstOrDefault(x => x.CardID == card.CardID);
        if (card == null) return;
        cardFactory.ReturnToPool(card);

    }
    private void RepositionCards()
    {
        Debug.Log("reosition");
        if (cardEntries.Count == 0) return;

        float totalWidth = 1000f; // chiều rộng bạn muốn căn giữa
        float spacing = 0f;

        if (cardEntries.Count > 1)
            spacing = totalWidth / (cardEntries.Count - 1);

        float startX = -totalWidth / 2f; // bắt đầu từ trái sang phải

        for (int i = 0; i < cardEntries.Count; i++)
        {
            float targetX = startX + i * spacing;
            // Giữ nguyên Y, chỉ di chuyển X
            Vector3 targetPos = new Vector3(targetX, 0f, 0f);

            cardEntries[i].transform
                .DOLocalMoveX(targetPos.x, 0.2f) // dùng local để relative với parent
                .SetEase(Ease.OutQuad);
        }
    }

}
