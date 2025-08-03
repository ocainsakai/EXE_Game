using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : GameTurner
{
    [SerializeField] private Transform deckContainer;
    [SerializeField] private Transform handContainer;
    [SerializeField] private GameObject cardPrf;
    public List<CardSDData> DeckData = new List<CardSDData>();

    public List<Card> CardsInHand { get; private set; } = new List<Card>();
    public List<Card> CardsInDeck { get; private set; } = new List<Card>();
    public void Initialize(IEnumerable<CardSDData> initDeck)
    {
        DeckData.Clear(); // Clear the existing deck
        DeckData.AddRange(initDeck);

        foreach (var card in DeckData)
        {
            var cardObject = Instantiate(cardPrf);
            cardObject.transform.SetParent(deckContainer);
            cardObject.transform.localPosition = Vector3.zero; // Reset position
            cardObject.gameObject.SetActive(false); // Ensure the card is active
            Card newCard = cardObject.GetComponent<Card>();
            newCard.GetComponent<CardDataHolder>().Data.Value = card;
            CardsInDeck.Add(newCard);
        }

        CardsInDeck = CardsInDeck.OrderBy(x => Random.value).ToList();

        for(int i = 0; i < 8 && i < CardsInDeck.Count; i++)
        {
            var card = CardsInDeck[i];
            card.gameObject.SetActive(true);
            card.transform.SetParent(handContainer); // Move to player's hand
            card.GetComponent<CardSelecter>().CanSelect = true; // Enable selection
            card.GetComponent<CardInputDecider>().CanSelect = true; // Enable selection
            CardsInHand.Add(card);
        }
    }
}
