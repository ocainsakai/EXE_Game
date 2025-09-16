using DG.Tweening;
using UnityEngine;

public class RoomView : MonoBehaviour
{
    [SerializeField] private Transform activeCardContainer;
    private Room room;

    public readonly float ROOM_WIDTH = 10;
    public readonly float CARD_WIDTH = 1;
    public void Bind(Room room)
    {
        this.room = room;
        room.OnCardAdded += HandleCardAdded;
        //room.OnCardRemoved += 
    }
    private void HandleCardAdded(Card card)
    {
        // Đưa card vào UI container
        card.transform.SetParent(activeCardContainer, false);

        // TODO: Animate vị trí
    }

    private void HandleRoomScored()
    {
        // Highlight, flash, shake animation
        Debug.Log("Room scored!");
    }
    public void UpdateView(int handSize)
    {
        for (int i = 0; i < room.Cards.Count; i++)
        {
            room.Cards[i].transform.DOLocalMove(GetPositionAtIndex(i, handSize), 0.25f);
        }
    }
    private Vector3 GetPositionAtIndex(int index, int Handsize)
    {
        var startX = -(ROOM_WIDTH - CARD_WIDTH) / 2;
        var dictant = (ROOM_WIDTH) / (Handsize+1);
        var x = startX + dictant * index;
        return new Vector3(x, 0);
    }
    public void Clear()
    {
        foreach (Transform child in activeCardContainer)
        {
            Destroy(child.gameObject);
        }
    }
}
