using UnityEngine;

public class CardDispacher : MonoBehaviour
{
    public DeckManager deck;
    public Room room;
    public DiscardPile discardPile;
    public UICardFactory cardFactory;

    public bool IsTestMode;

    private void Start()
    {
        if (IsTestMode)
        {
            foreach (var card in deck.OriginCards)
            {
                cardFactory.GetOrCreateCard(card);
            }
            ActiveRoom();
        }
    }


    public void ActiveRoom()
    {
        deck.CreateRuntimeDeck();
        int cardNeed = room.RoomSize;
        for (int i = 0; i < cardNeed; i++)
        {
            var card = deck.DrawCard();
            room.Add(card);
        }
    }

}
