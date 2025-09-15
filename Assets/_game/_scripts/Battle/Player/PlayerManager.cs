using CardSystem;
using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private DeckManager deckManager;
    [SerializeField] private RoomController roomController;
    [SerializeField] private PlayerStateLoader playerStateLoader;
    [SerializeField] private List<CardData> startingCards;
    [SerializeField] private PlayerButton PlayerButton;

    private IGameManager gameManager;
    private ISceneLoader sceneLoader;

    [Inject]
    public void Construct(IGameManager gameManager, ISceneLoader sceneLoader)
    {
        this.gameManager = gameManager;
        this.sceneLoader = sceneLoader;
    }

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
    [ContextMenu("test create cards")]
    public void TestCreateCards()
    {
        deckManager.CreateCards(startingCards);
        playerStateLoader.LoadConfig();
        StartRoom();
    }
    private void StartRoom()
    {
        roomController.StartRoom();
        deckManager.ShuffeDeck();

        DrawCards();
        roomController.UpdateView(CurrentState.HandSize);
    }

    //private void DrawCards(int amount)
    //{
    //    var cards = deckManager.DrawCards(amount);
    //    cards.ForEach(card => roomController.AddCardView(card));
    //}
    private void DrawCards()
    {
        while (roomController.ActiveCards.Count < CurrentState.HandSize)
        {
            roomController.AddCardView(deckManager.DrawCard());
        }
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
