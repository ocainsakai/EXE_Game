using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIRoom : MonoBehaviour
{
    [SerializeField] private UICardFactory cardFactory;
    //[SerializeField] private Room room;
    private List<CardEntry> cardEntries = new List<CardEntry>();



    public void UpdateInteract(bool canSelect)
    {
        //Debug.Log("UI update: " + canSelect);
        foreach (CardEntry entry in cardEntries)
        {
            entry.SetButton(canSelect);
        }
    }

    public void UpdateCards(List<Card> newCards)
    {
        // Tạo dictionary để lookup nhanh
        var cardEntryDict = cardEntries.ToDictionary(entry => entry.CardID, entry => entry);

        // Tìm cards mới cần thêm vào
        var cardsToAdd = newCards.Where(card => !cardEntryDict.ContainsKey(card.CardID)).ToList();

        // Tìm cards cũ cần remove
        var newCardIds = new HashSet<SerializableGuid>(newCards.Select(c => c.CardID));
        var entriesToRemove = cardEntries.Where(entry => !newCardIds.Contains(entry.CardID)).ToList();

        // Remove các cards không còn trong newCards
        foreach (var entry in entriesToRemove)
        {
            cardEntries.Remove(entry);
            cardFactory.ReturnToPool(entry);
        }

        // Add cards mới
        if (cardsToAdd.Count > 0)
        {
            AddCardToRoom(cardsToAdd);
        }

        // Rebuild dictionary sau khi add
        cardEntryDict = cardEntries.ToDictionary(entry => entry.CardID, entry => entry);

        // Reorder theo thứ tự của newCards
        cardEntries = newCards
            .Where(card => cardEntryDict.ContainsKey(card.CardID))
            .Select(card => cardEntryDict[card.CardID])
            .ToList();

        RepositionCards();
    }

    public void DiscardFormRoom(List<Card> cards)
    {
        var cardIds = new HashSet<SerializableGuid>(cards.Select(c => c.CardID));
        var entriesToRemove = cardEntries.Where(e => cardIds.Contains(e.CardID)).ToList();

        foreach (var entry in entriesToRemove)
        {
            cardEntries.Remove(entry);
            cardFactory.ReturnToPool(entry);
        }

        RepositionCards();
    }

    public void AddCardToRoom(Card card)
    {
        //Debug.Log(card.ToString() + "UI");
        var entry = cardFactory.GetOrCreateCard(card);
        if (cardEntries.Contains(entry)) return;
        AddCardToRoom(entry);
        RepositionCards();
    }
    public void AddCardToRoom(List<Card> cards)
    {
        foreach (var card in cards)
        {
            AddCardToRoom(card);
        }
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
    public void DiscardFromRoom(Card card)
    {
        var entry = cardFactory.GetOrCreateCard(card);
        if (cardEntries.Contains(entry)) return;
        DiscardFromRoom(entry);
    }
    public void DiscardFromRoom(CardEntry card)
    {
        if (card == null) return;

        cardEntries.Remove(card);
        cardFactory.ReturnToPool(card);
        RepositionCards();
    }

    private void RepositionCards()
    {
        //Debug.Log("Reposition: " + cardEntries.Count + " cards");
        if (cardEntries.Count == 0) return;

        float totalWidth = 1000f;
        float spacing = 0f;

        if (cardEntries.Count > 1)
            spacing = totalWidth / (cardEntries.Count - 1);

        float startX = -totalWidth / 2f;

        for (int i = 0; i < cardEntries.Count; i++)
        {
            float targetX = startX + i * spacing;
            Vector3 targetPos = new Vector3(targetX, 0f, 0f);

            cardEntries[i].transform
                .DOLocalMoveX(targetPos.x, 0.2f)
                .SetEase(Ease.OutQuad);
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}