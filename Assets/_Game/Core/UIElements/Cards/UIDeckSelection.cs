using CardSystem;
using System;
using System.Collections.Generic;
using _Game.Core;
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
    private DeckData CurrentDetailDeck => GameInstance.Singleton.GetDeckData(currentDetailDeckId);
    private int Max => GameInstance.Singleton.deckData.Length; 
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
        if (CurrentDetailDeck != null && CurrentDetailDeck.CheckUnlocked)
            PlayerSave.SetSelectedDeck(currentDetailDeckId);
    }

    private void OnRightBtnClicked()
    {
        currentDetailDeckId++;
        if (currentDetailDeckId >= Max)
            currentDetailDeckId = 0;
        UpdateDeckView();
    }

    private void OnLeftBtnClicked()
    {
        currentDetailDeckId--;
        if (currentDetailDeckId < 0)
            currentDetailDeckId = Max - 1;
        UpdateDeckView();
    }

    private void UpdateDeckView()
    {
        deckView.gameObject.SetActive(true);
        var deck = CurrentDetailDeck;
        if (deck != null)
        {
            deckView.SetDeckName(deck.DeckName);
            if (deck.CheckUnlocked)
                deckView.SetCardBack(deck.DeckCover != null ? deck.DeckCover : defaultCardBack);
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
