using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRoom : MonoBehaviour
{

    private List<CardEntry> cardEntries = new List<CardEntry>();
    private Queue<IEnumerator> animationQueue = new Queue<IEnumerator>();

    public void UpdateIndex(IEnumerable<Card> cards)
    {
        cardEntries.Clear();
        cardEntries = UICardManager.Singleton.GetCards(cards);
        RepositionCards();
    }
    public void Add(Card card)
    {
        var entry = UICardManager.Singleton.GetCard(card);
        if (entry == null || cardEntries.Contains(entry)) return;
        entry.transform.SetParent(transform);
        cardEntries.Add(entry);


        StartCoroutine(AddAnimation(entry));
    }
    private IEnumerator ProcessQueue()
    {
        while (animationQueue.Count > 0)
        {
            yield return StartCoroutine(animationQueue.Dequeue());
        }
        //RepositionCards();
    }
    public void AddRange(IEnumerable<Card> cards)
    {
        if (cards == null) return;
        foreach (var card in cards)
        {
            var entry = UICardManager.Singleton.GetCard(card);
            if (entry == null || cardEntries.Contains(entry)) return;
            cardEntries.Add(entry);
            entry.transform.SetParent(transform);

            animationQueue.Enqueue(AddAnimation(entry));
        }
        StartCoroutine(ProcessQueue());
    }

    private IEnumerator AddAnimation(CardEntry cardEntry)
    {
        yield return cardEntry.transform
            .DOMove(transform.position, 0.1f)
            .SetEase(Ease.OutQuad);
    }
    public void Remove(Card card)
    {
        var entry = UICardManager.Singleton.GetCard(card);
        if (entry == null) return;
        cardEntries.Remove(entry);
    }
    private void RepositionCards()
    {
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
