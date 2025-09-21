using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    [SerializeField] private RoomView roomView;
    private Room room = new Room();

    public IReadOnlyList<Card> ActiveCards => room.Cards;

    public void StartRoom()
    {
        room.StartRoom();
        roomView.Bind(room);
        
    }
    public void EndRoom()
    {

    }

    public void AddCardView(Card cardController)
    {
        if (cardController == null) return;
        room.AddCard(cardController);
        cardController.onSelectChange += SelectHandle;
        cardController.IsSelecting = false;
        
    }

    private void SelectHandle()
    {
        // update can select
        Card.CanSelect = room.SelectCount < 5;
        // update poker hand
    }

    public void UpdateView(int handSize)
    {
        Debug.Log("asd");
        roomView.UpdateView(handSize);
    }

    internal void Discards()
    {
        var cards = room.Cards.Where(x => x.IsSelecting).ToList();
        foreach (var card in cards)
        {
            room.RemoveCard(card); 
            card.MoveTo(Card.discardPile);
        }
        SelectHandle();
    }
}
