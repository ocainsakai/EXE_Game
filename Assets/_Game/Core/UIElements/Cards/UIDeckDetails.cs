using CardSystem;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class UIDeckDetails : MonoBehaviour
{
    private List<Card> currentDeck;
    [SerializeField] DeckManager deckManager;

    [SerializeField] GameObject deckInfo;
    [SerializeField] GameObject baseCards;
    [SerializeField] GameObject rankCount;
    [SerializeField] GameObject content;

    [SerializeField] AtributeEntry atributeVertical;
    [SerializeField] RankAttributeEntry atributeHorizontal;

    [SerializeField] List<Sprite> baseIcons;

    private List<char> ranks = new List<char>() { 'A' , '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'J', 'Q', 'K', };

    public void Toggle()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
    private void OnEnable()
    {
        SetDeck(deckManager.Cards.ToList());
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
        // ví dụ: hiển thị tổng số face cards và number cards
        int faceCount = currentDeck.GetFaceCardCount();
        int numberCount = currentDeck.GetNumberCardCount();
        int aceCount = currentDeck.GetAceCount();

        //var text = content.GetComponentInChildren<TextMeshProUGUI>();
        //text.text = $"Faces: {faceCount}\n" +
        //            $"Numbers: {numberCount}\n" +
        //            $"Aces: {aceCount}";
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
                return currentDeck.GetSuitCount(CardSystem.CardSuit.Hearts);
            case 5:
                return currentDeck.GetSuitCount(CardSystem.CardSuit.Diamonds);
            case 6:
                return currentDeck.GetSuitCount(CardSystem.CardSuit.Clubs);
            case 7:
                return currentDeck.GetSuitCount(CardSystem.CardSuit.Spades);

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
}
