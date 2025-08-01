using UniRx;
using UnityEngine;

public class CardContainer : MonoBehaviour
{
    public ReactiveCollection<Card> Cards { get; private set; } = new ReactiveCollection<Card>();

    public void Add(Card card)
    {
        Cards.Add(card);
    }
    public void Remove(Card card)
    {
        if (Cards.Contains(card))
        {
            Cards.Remove(card);
        }
    }
}
