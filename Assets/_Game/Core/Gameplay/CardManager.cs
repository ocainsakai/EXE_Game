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
        deck.Clear();
        discardPile.Clear();
        room.Clear();
        
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

    // Ra lệnh đánh các lá bài đã chọn trên tay
    public void PlaySelectedCards()
    {
        // Lấy danh sách copy để tránh lỗi khi thay đổi collection
        var cardsToPlay = room.SelectedCards.ToList(); 
        
        if (!cardsToPlay.Any())
        {
            Debug.Log("No cards selected to play.");
            return;
        }

        foreach (var card in cardsToPlay)
        {
            // 1. Thực hiện hành động của lá bài
            StartCoroutine(card.Active());

            // 2. Di chuyển lá bài từ Room sang DiscardPile
            room.Remove(card);
            discardPile.Add(card);
        }
    }

    // Xáo trộn Mộ Bài và đưa lại vào Deck
    private void ReshuffleDiscardIntoDeck()
    {
        Debug.Log("Reshuffling discard pile into deck...");
        var cardsFromDiscard = discardPile.TakeAllCards();
        deck.AddCardsAndShuffle(cardsFromDiscard);
    }
    public void DiscardSelectedCards()
    {
        var cardsToDiscard = room.SelectedCards;
        if (cardsToDiscard == null || cardsToDiscard.Count == 0)
        {
            Debug.LogWarning("[CardManager] Discard failed: No cards selected.");
            return;
        }

        foreach (var card in cardsToDiscard.ToList())
        {
            room.Remove(card);
            discardPile.Add(card);
        }

        Debug.Log($"[CardManager] Discarded {cardsToDiscard.Count} card(s).");
        room.UnselectAll();
    }
}
