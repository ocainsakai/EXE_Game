using UniRx;
using UnityEngine;

public class CardContainer : MonoBehaviour
{
    public ReactiveCollection<Card> Cards { get; private set; } = new ReactiveCollection<Card>();
    private void Awake()
    {
        // Subscribe to the CardDraggable swap event
        CardDraggable.OnCardSwapped += Swap;
    }
    public void Add(Card card)
    {
        Cards.Add(card);
    }

    private void Swap(Card sourceCard, Card targetCard)
    {
        if (sourceCard == null || targetCard == null || sourceCard == targetCard)
            return;

        int sourceIndex = Cards.IndexOf(sourceCard);
        int targetIndex = Cards.IndexOf(targetCard);

        if (sourceIndex == -1 || targetIndex == -1)
            return;
        Debug.Log($"Swapping {sourceCard.name} at index {sourceIndex} with {targetCard.name} at index {targetIndex}");
        Cards[sourceIndex] = targetCard;
        Cards[targetIndex] = sourceCard;
        
    }

    public void Remove(Card card)
    {
        if (Cards.Contains(card))
        {
            Cards.Remove(card);
        }
    }
}
