using CardSystem;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDeckSelection : MonoBehaviour
{
    [SerializeField] private DeckEntry deckView;
    [SerializeField] private Button leftBtn;
    [SerializeField] private Button rightBtn;
    [SerializeField] private Sprite defaultCardBack;
    [SerializeField] private Sprite lockedCardBack;

    [SerializeField] private Button selectBtn;
    private int currentDetailDeckId;
    private DeckData currentDetailDeck => GameInstance.Singleton.GetDeckData(currentDetailDeckId);
    private int max => GameInstance.Singleton.deckDatas.Length; 
    private void OnEnable()
    {
        leftBtn.onClick.AddListener(OnLeftBtnClicked);
        rightBtn.onClick.AddListener(OnRightBtnClicked);
        selectBtn.onClick.AddListener(OnSelectBtnClicked);
        currentDetailDeckId = PlayerSave.GetSelectedDeck();
        UpdateDeckView();

    }
   
    private void OnDisable()
    {
        leftBtn.onClick.RemoveListener(OnLeftBtnClicked);
        rightBtn.onClick.RemoveListener(OnRightBtnClicked);
        selectBtn.onClick.RemoveListener(OnSelectBtnClicked);
    }
    private void OnSelectBtnClicked()
    {
        if (currentDetailDeck != null && currentDetailDeck.CheckUnlocked)
            PlayerSave.SetSelectedDeck(currentDetailDeckId);
    }

    private void OnRightBtnClicked()
    {
        currentDetailDeckId++;
        if (currentDetailDeckId >= max)
            currentDetailDeckId = 0;
        UpdateDeckView();
    }

    private void OnLeftBtnClicked()
    {
        currentDetailDeckId--;
        if (currentDetailDeckId < 0)
            currentDetailDeckId = max - 1;
        UpdateDeckView();
    }

    private void UpdateDeckView()
    {
        deckView.gameObject.SetActive(true);
        var deck = currentDetailDeck;
        if (deck != null)
        {
            deckView.SetDeckName(deck.DeckName);
            if (deck.CheckUnlocked)
                deckView.SetCardBack(deck.CardBack != null ? deck.CardBack : defaultCardBack);
            else
                deckView.SetCardBack(lockedCardBack);
        }
        else
        {
            deckView.SetDeckName("No Deck");
            deckView.SetCardBack(defaultCardBack);
        }
    }

}
