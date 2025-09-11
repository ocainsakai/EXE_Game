using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    [SerializeField] private RoomView roomView;

    private Room room = new Room();

    public IReadOnlyList<CardController> ActiveCards => room.Cards;

    public void StartRoom()
    {
        room.StartRoom();
        roomView.Bind(room);

    }
    public void EndRoom()
    {

    }

    public void AddCardView(CardController cardController)
    {
        room.AddCard(cardController);
    }

    public void UpdateView(int handSize)
    {
        roomView.UpdateView(handSize);
    }
}
