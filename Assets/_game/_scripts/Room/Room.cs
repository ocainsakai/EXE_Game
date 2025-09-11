using System;
using System.Collections.Generic;

public class Room
{
    private readonly List<CardController> cards = new();

    public IReadOnlyList<CardController> Cards => cards;

    public event Action<CardController> OnCardAdded;
    public event Action<CardController> OnCardRemoved;
    public event Action OnRoomScored;
    public event Action OnRoomCleared;

    public void StartRoom()
    {
        cards.Clear();
    }

    public void AddCard(CardController card)
    {
        cards.Add(card);
        OnCardAdded?.Invoke(card);
    }

    public void RemoveCard(CardController card)
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
