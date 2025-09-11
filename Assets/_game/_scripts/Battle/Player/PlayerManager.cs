using CardSystem;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private DeckManager deckManager;
    [SerializeField] private RoomController roomController;
    [SerializeField] private PlayerStateLoader playerStateLoader;
    [SerializeField] private List<CardData> startingCards;


    private IGameManager gameManager;
    private ISceneLoader sceneLoader;

    [Inject]
    public void Construct(IGameManager gameManager, ISceneLoader sceneLoader)
    {
        this.gameManager = gameManager;
        this.sceneLoader = sceneLoader;
    }
    private PlayerState _currentState;
    public PlayerState CurrentState => _currentState;

    public void StartRun()
    {
        // if temp logic
        _currentState = playerStateLoader.basic.state;
        Debug.Log("Start run exe");
        sceneLoader.LoadSceneName("Map").Execute();
    }
    [ContextMenu("test create cards")]
    private void TestCreateCards()
    {
        deckManager.CreateCards(startingCards);
        StartRoom();
    }
    private void StartRoom()
    {
        deckManager.ShuffeDeck();
        var cards = deckManager.DrawCards(CurrentState.HandSize);
        roomController.StartRoom();
        cards.ForEach(card => roomController.AddCardView(card));
        Debug.Log(cards.Count);
        roomController.UpdateView(CurrentState.HandSize);
    }
}
