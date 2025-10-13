using CardSystem;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    private List<Card> originCards=new();
    private List<Card> cards=new();
    public IReadOnlyCollection<Card> OriginCards => originCards;
    public IReadOnlyCollection<Card> Cards => cards;

    public Sprite DeckCover { get; private set; }
    
    public bool IsTestMode;

    public DeckData testDeck;
    private void Awake()
    {
        if (IsTestMode)
        {
            originCards = (CreateCards(testDeck.Cards).ToList());
            DeckCover = testDeck.DeckCover;
            return;
        }

        var deckData = GameInstance.Singleton.GetDeckData(PlayerSave.GetSelectedDeck());
        if (deckData == null)
        {
            Debug.LogError("DeckData is null");
            return;
        }

        Debug.Log(deckData.ToString() + $"   {deckData.Cards.Count}");
        originCards = (CreateCards(deckData.Cards).ToList());
        DeckCover = deckData.DeckCover;
    }
    public bool DestroyCard(SerializableGuid cardID)
    {
        if (originCards.Select(x => x.CardID).Contains(cardID))
        {
            var item = originCards.FirstOrDefault(x => x.CardID == cardID);
            originCards.Remove(item);
            return true;
        }
        return false;
    }

    public Card CreateCard(CardData data)
    {
        Card card = new(data);
        originCards.Add(card);
        return card;
    }

    public IEnumerable<Card> CreateCards(IEnumerable<CardData> cardsData)
    {
        foreach (var cardData in cardsData)
        {
           yield return CreateCard(cardData);
        }
    }

    public void CreateRuntimeDeck()
    {
        cards.Clear();
        cards = new List<Card>(originCards);
        ShuffleDeck();
    }

    public Card DrawCard()
    {
        var card = cards.FirstOrDefault();
        cards.Remove(card);
        return card;
    }
    public void ShuffleDeck()
    {
        for (int i = cards.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            (cards[i], cards[randomIndex]) = (cards[randomIndex], cards[i]);
        }
    }

}
