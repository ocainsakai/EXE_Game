
using UnityEngine;

public class PlayerManager : BaseManager<PlayerManager>
{
    [SerializeField] private DeckManager deckManager;
    [SerializeField] private RoomController roomController;
    [SerializeField] private PlayerButton PlayerButton;

    private void OnEnable()
    {
        PlayerButton.onDiscardButtonClicked += DiscardHandle;
        PlayerButton.onPlayButtonClicked += PlayHandle;
        PlayerButton.onSortButtonClicked += SortHandle;
    }

  
    private void OnDisable()
    {
        PlayerButton.onDiscardButtonClicked -= DiscardHandle;
        PlayerButton.onPlayButtonClicked -= PlayHandle;
        PlayerButton.onSortButtonClicked -= SortHandle;
    }



    private PlayerState _currentState;
    public PlayerState CurrentState
    {
        get => _currentState;
        set
        {
            _currentState = value;
        }
    }

    public void StartRoom()
    {
        roomController.StartRoom();
        deckManager.ShuffeDeck();

        DrawCards();
        roomController.UpdateView(CurrentState.HandSize);
    }

    private void DrawCards(int amount)
    {
        var cards = deckManager.DrawCards(amount);
        cards.ForEach(card => roomController.AddCardView(card));
    }
    private void DrawCards()
    {
        int amount = CurrentState.HandSize - roomController.ActiveCards.Count;
        DrawCards(amount);
    }
    private void SortHandle()
    {
        if (roomController == null) return;

    }

    private void PlayHandle()
    {
        if (roomController == null) return;

    }

    private void DiscardHandle()
    {
        if(roomController == null || roomController.ActiveCards.Count <= 0) return;
        roomController.Discards();
        DrawCards();
        roomController.UpdateView(CurrentState.HandSize);

    }
}
