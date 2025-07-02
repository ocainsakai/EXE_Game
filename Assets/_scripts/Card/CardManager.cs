using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;

public class CardManager : MonoBehaviour
{
    public DeckData deckData;
    public CardFactory cardFactory;
    public MoveCards moveCards;
    public InputManager inputManager;

    private List<Card> allCards = new List<Card>();

    private void Awake()
    {
        cardFactory = GetComponent<CardFactory>();
    }

    private void Start()
    {
        inputManager.Draw.Subscribe(_ => DrawHand());

        InitializeDeck();
    }

    public void DrawHand()
    {
        List<Card> cards = new List<Card>();
        cards = allCards.GetRange(0, 8);
        moveCards.MoveTo(MoveCards.Place.Hand, cards);
    }
    private void InitializeDeck()
    {
        foreach (var item in deckData.startingCards)
        {
            var card = cardFactory.CreateCard(item, deckData.CardBack);
            card.gameObject.SetActive(true);
            allCards.Add(card);
        }
        allCards = allCards.OrderBy(x => Random.value).ToList();
    }
}
