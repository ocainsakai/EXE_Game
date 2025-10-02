using CardSystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckManager : BaseCardPile
{
    [SerializeField] UICardManager cardManager;
    [SerializeField] Transform cardContainer;
    [SerializeField] private Image deckCover;

    private List<Card> cards = new List<Card>();

    private void Awake()
    {
        // Lấy deck đã chọn từ PlayerSave thông qua GameInstance
        var deckData = GameInstance.Singleton.GetDeckData(PlayerSave.GetSelectedDeck());
        if (deckData == null)
        {
            Debug.LogError("DeckData is null!");
            return;
        }

        // Gán cover cho UI
        if (deckCover != null)
            deckCover.sprite = deckData.DeckCover;

        // Tạo các lá bài trong deck
        CreateCards(deckData.Cards);

        // Shuffle deck ngay từ đầu nếu cần
        ShuffleDeck();
    }

    public void CreateCards(IEnumerable<CardData> cardDatas)
    {
        cards.Clear();

        foreach (var data in cardDatas)
        {
            Card card = new Card(data);
            cards.Add(card);

            // Spawn UI
            if (cardManager != null)
                cardManager.Add(card, cardContainer);
        }
    }

    public void ShuffleDeck()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            int rnd = Random.Range(i, cards.Count);
            (cards[i], cards[rnd]) = (cards[rnd], cards[i]);
        }
    }

    public List<Card> Cards => cards;
}
