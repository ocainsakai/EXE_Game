using CardSystem;
using System;
using System.Collections.Generic;
using _Game.Core;
using TMPro;
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
        {
            PlayerSave.SetSelectedDeck(currentDetailDeckId);
            selectBtnText.text = "Đã chọn";
        }
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

    // --- Thêm dòng này ở đầu class của bạn ---
    [SerializeField] private TextMeshProUGUI selectBtnText;
// Sau đó kéo component TextMeshProUGUI của nút Select vào đây trong Inspector

    private void UpdateDeckView()
    {
        deckView.gameObject.SetActive(true);
        var deck = CurrentDetailDeck;

        // --- 1. Guard Clause (Thoát sớm) ---
        // Nếu không có deck nào được chọn, set trạng thái "No Deck" và thoát
        if (deck == null)
        {
            deckView.SetDeckName("Không Có");
            deckView.SetCardBack(defaultCardBack);
            selectBtn.gameObject.SetActive(false); // Ẩn nút select đi
            return; // Thoát khỏi hàm
        }

        // --- Logic chính (chỉ chạy khi 'deck' chắc chắn không null) ---

        deckView.SetDeckName(deck.DeckName);

        // --- 2. Logic được làm phẳng ---
        if (deck.CheckUnlocked)
        {
            // Deck đã mở khóa
            var cardBack = (deck.DeckCover != null) ? deck.DeckCover : defaultCardBack;
            deckView.SetCardBack(cardBack);

            // --- 3. Dùng toán tử ba ngôi (Ternary Operator) ---
            bool isSelected = (deck.DeckID == PlayerSave.GetSelectedDeck());
            selectBtnText.text = isSelected ? "Đã chọn" : "Chọn";
        
            selectBtn.gameObject.SetActive(true); // Hiển thị nút
        }
        else
        {
            // Deck bị khóa
            deckView.SetCardBack(lockedCardBack);
            selectBtn.gameObject.SetActive(false); // Ẩn nút
        }
    }

}
