using CardSystem;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DeckManager : MonoBehaviour
{
    [SerializeField] private Transform deckCover;
    private List<Card> originCards=new();
    public IReadOnlyCollection<Card> OriginCards => originCards;
    private void Awake()
    {
        var deckData = GameInstance.Singleton.GetDeckData(PlayerSave.GetSelectedDeck());
        if (deckData == null)
        {
            Debug.LogError("DeckData is null");
            return;
        }

        Debug.Log(deckData.ToString() + $"   {deckData.Cards.Count}");
        originCards = (CreateCards(deckData.Cards).ToList());

        if (deckCover != null)
        {
            deckCover.GetComponent<Image>().sprite = deckData.DeckCover;
        }
        
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

    public void CreateCards(IEnumerable<CardData> cardDatas)
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

    public List<Card> Cards => cards;
}
