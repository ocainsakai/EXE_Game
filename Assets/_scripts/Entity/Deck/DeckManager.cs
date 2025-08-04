using UnityEngine;

public class DeckManager : MonoBehaviour
{
    [SerializeField] public CardList deckList;
    [SerializeField] public CardDatabase standardCards;

    public void InitializeDeck()
    {
        var cards = CardFactory.Instance.GetCards(standardCards.Cards);
        deckList.cards.Clear();
        deckList.cards.AddRange(cards);
    }
}
