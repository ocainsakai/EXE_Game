using System;
using System.Collections.Generic;
using System.Linq;

public class Room
{
    private readonly List<Card> cards = new();

    public IReadOnlyList<Card> Cards => cards;

    public event Action<Card> OnCardAdded;
    public event Action<Card> OnCardRemoved;
    public event Action OnRoomScored;
    public event Action OnRoomCleared;

    public int SelectCount => cards.Where(x => x.IsSelecting).Count();
    public void StartRoom()
    {
        cards.Clear();
    }

    public void AddCard(Card card)
    {
        cards.Add(card);
        OnCardAdded?.Invoke(card);
    }

    public void RemoveCard(Card card)
    {
        if (cards.Remove(card))
            OnCardRemoved?.Invoke(card);
    }

    public void ScoreRoom()
    {
        // TODO: gọi Evaluator tính điểm
        OnRoomScored?.Invoke();
    }

    public void EndRoom()
    {
        cards.Clear();
        OnRoomCleared?.Invoke();
    }
}
