using CardSystem;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIDeckDetails : MonoBehaviour
{
    private List<Card> currentDeck;
    [SerializeField] DeckManager deckManager;

    [SerializeField] GameObject deckInfo;
    [SerializeField] GameObject baseCards;
    [SerializeField] GameObject rankCount;
    [SerializeField] GameObject content;
    [SerializeField] GameObject cardPrefab;

    [SerializeField] AtributeEntry atributeVertical;
    [SerializeField] RankAttributeEntry atributeHorizontal;

    [SerializeField] Button remainingButton;
    [SerializeField] Button fullDeckButton;

    [SerializeField] List<Sprite> baseIcons;

    private List<char> ranks = new List<char>() { 'A' , '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'J', 'Q', 'K', };

    public void Toggle()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
    private void OnEnable()
    {
        //SetDeck(deckManager.);
    }

    public void SetDeck(List<Card> deck)
    {
        currentDeck = deck;
        UpdateDeckDetails();
    }

    void UpdateDeckDetails()
    {
        if (currentDeck == null) return;

        UpdateDeckInfo();
        UpdateBaseCards();
        UpdateRankCount();
        UpdateContent();
    }

    private void UpdateContent()
    {
        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }

        // sort deck theo Balatro style
        var sortedDeck = currentDeck
            .Select(c => c.CardData)   // từ Card → CardData
            .SortForDisplay();

        foreach (var cardData in sortedDeck)
        {
            GameObject cardGO = Instantiate(cardPrefab, content.transform);
            var ui = cardGO.GetComponent<CardUI>();
            if (ui != null)
            {
                ui.SetCard(card.Art); // Card.Data = CardData ScriptableObject (Ace_of_Clubs, v.v.)
            }
        }
    }

    private void UpdateDeckInfo()
    {
        int total = currentDeck.Count;
        deckInfo.GetComponentInChildren<TextMeshProUGUI>().text = $"Deck Info ({total} cards)";
    }

    void UpdateBaseCards()
    {
        foreach (Transform child in baseCards.transform)
        {
            Destroy(child.gameObject);
        }

        // giả sử baseIcons = [Spade, Heart, Diamond, Club]
        for (int i = 0; i < baseIcons.Count; i++)
        {

            GameObject icon = Instantiate(atributeVertical.gameObject, baseCards.transform);
            icon.SetActive(true);
            icon.GetComponent<AtributeEntry>().SetData(baseIcons[i], GetBaseCount(i));
        }
    }
    int GetBaseCount(int index)
    {
        switch (index)
        {
            case 1: 
                return currentDeck.GetAceCount();
            case 2: 
                return currentDeck.GetFaceCardCount();
            case 3: // Clubs
                return currentDeck.GetNumberCardCount();
            case 4: // Others (Joker, etc.)
                return currentDeck.GetSuitCount(CardSuit.Hearts);
            case 5:
                return currentDeck.GetSuitCount(CardSuit.Diamonds);
            case 6:
                return currentDeck.GetSuitCount(CardSuit.Clubs);
            case 7:
                return currentDeck.GetSuitCount(CardSuit.Spades);

            default:
                return 0;
        }   
    }
    void UpdateRankCount()
    {
        foreach (Transform child in rankCount.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < ranks.Count; i++)
        {
           
            var rankEnum = ((i==0) ? CardRank.Ace : (CardRank)(i + 1));
            int count = currentDeck.GetRankCount(rankEnum);

            GameObject icon = Instantiate(atributeHorizontal.gameObject, rankCount.transform);
            icon.SetActive(true);
            icon.GetComponent<RankAttributeEntry>().SetAttribute(ranks[i].ToString(), count);
        }
    }

    public void ShowRemainingCards()
    {
        // ví dụ: chỉ hiển thị những lá bài chưa được rút
        Debug.Log("Show Remaining Cards");

        var remaining = deckManager.Cards; // tuỳ logic bạn đang giữ bài ở đâu
        SetDeck(remaining.ToList());
    }

    public void ShowFullDeck()
    {
        Debug.Log("Show Full Deck");

        var fullDeck = deckManager.Cards; // toàn bộ 52 lá
        SetDeck(fullDeck.ToList());
    }
}
