using System.Collections.Generic;
using System.Linq;
using _Game.Addons.Deck.Scripts;
using _Game.Addons.Deck.Scripts.Card;
using _Game.Addons.Deck.Scripts.CardCollection;
using UnityEngine;

public class CardManager : MonoBehaviour
{
   [Header("Card Collections")]
    [SerializeField] private DeckManager deck;
    [SerializeField] private Room room; 
    [SerializeField] private DiscardPile discardPile;
    [SerializeField] private UICardFactory uiCardFactory;
    public List<CardRuntime> SelectedCards => room.SelectedCards;
    public void StartBattle(List<CardData> allCardDataInDeck)
    {
        Clear();
        
        // Tạo CardRuntime từ CardData
        var startingCards = allCardDataInDeck.Select(data => new CardRuntime(data)).ToList();
        uiCardFactory.GetOrCreateCardEntries(startingCards);
        deck.AddCardsAndShuffle(startingCards);
    }

    public void DrawHand()
    {
        int cardNeed = room.roomSize - room.Cards.Count;
        Debug.Log($"[PlayerActionController] Drawing {cardNeed} card(s).");
        DrawCards(cardNeed);
    }
    // Ra lệnh rút một số lượng bài
    public void DrawCards(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            // Nếu Deck hết bài, xáo trộn Mộ Bài và đưa lại vào Deck
            if (deck.CardCount == 0)
            {
                ReshuffleDiscardIntoDeck();
                // Nếu sau khi xáo trộn vẫn hết bài thì dừng
                if (deck.CardCount == 0)
                {
                    Debug.Log("Out of cards everywhere!");
                    return;
                }
            }
            
            CardRuntime cardToDraw = deck.Draw();
            Debug.Log($"{cardToDraw is null} draw {i} card");
            room.Add(cardToDraw); // Room sẽ tự động phát event cho UIRoom
        }
        room.CurrentSort();
    }
    // Xáo trộn Mộ Bài và đưa lại vào Deck
    private void ReshuffleDiscardIntoDeck()
    {
        Debug.Log("Reshuffling discard pile into deck...");
        var cardsFromDiscard = discardPile.TakeAllCards();
        deck.AddCardsAndShuffle(cardsFromDiscard);
    }
    public bool DiscardSelectedCards()
    {
        var cardsToDiscard = room.Discards();
        if (cardsToDiscard == null || cardsToDiscard.Count == 0)
        {
            Debug.LogWarning("[CardManager] Discard failed: No cards selected or returned null.");
            return false;
        }

        
        foreach (var card in cardsToDiscard)
        {
            discardPile.Add(card);
        }

        return true;
    }

    public void Clear()
    {
        deck.Clear();
        discardPile.Clear();
        room.Clear(); 
        uiCardFactory.DestroyAll();
    }
}
