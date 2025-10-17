using DG.Tweening;
using System;
using _Game.Addons.Deck.Scripts;
using _Game.Addons.Deck.Scripts.Card;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CardEntry : MonoBehaviour
{
    [FormerlySerializedAs("_art")] [SerializeField] private Image art;
    [FormerlySerializedAs("_selectBtn")] [SerializeField] private Button selectBtn;

    [FormerlySerializedAs("CardID")] public SerializableGuid cardID;
    public CardRuntime CardRuntime { get; private set; }
    public bool IsSelected { get; private set; } // Trạng thái này sẽ được đồng bộ từ Card
    public Action OnCardClicked; // Giữ lại nếu bạn cần thông báo cho một manager khác


    public void SetCard(CardRuntime cardRuntime)
    {

        this.CardRuntime = cardRuntime;
        cardID = cardRuntime.CardID;

        if (art != null)
        {
            art.sprite = cardRuntime.Art;
        }
    }

    public void SetButton(bool canSelect)
    {
        selectBtn.interactable = canSelect;
    }

    private void OnEnable()
    {
        if (selectBtn != null)
        {
            // Đơn giản hóa listener, chỉ gọi một hàm
            selectBtn.onClick.AddListener(HandleClick);
        }
    }

    private void OnDisable()
    {
        // Hủy đăng ký listener của button
        if (selectBtn != null)
        {
            selectBtn.onClick.RemoveAllListeners();
        }
    }

    // Hàm được gọi khi button được nhấn
    private void HandleClick()
    {
        OnCardClicked?.Invoke(); 
        Debug.Log($"{gameObject} + {gameObject.name} + HandleClick ");
        CardRuntime.IsSelected = !CardRuntime.IsSelected; 
        IsSelected = CardRuntime.IsSelected;

        // 2. Cập nhật giao diện dựa trên trạng thái mới
        float targetY = IsSelected ? 50f : 0f;
        transform.DOLocalMoveY(targetY, 0.2f).SetEase(Ease.OutQuad);
    }
}