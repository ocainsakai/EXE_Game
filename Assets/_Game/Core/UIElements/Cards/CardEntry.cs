using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class CardEntry : MonoBehaviour
{
    [SerializeField] private Image _art;
    [SerializeField] private Button _selectBtn;

    public SerializableGuid CardID;
    public Card Card { get; private set; }
    public bool IsSelected { get; private set; } // Trạng thái này sẽ được đồng bộ từ Card
    public Action OnCardClicked; // Giữ lại nếu bạn cần thông báo cho một manager khác


    public void SetCard(Card card)
    {

        this.Card = card;
        CardID = card.CardID;

        if (_art != null)
        {
            _art.sprite = card.Art;
        }
    }

    public void SetButton(bool canSelect)
    {
        _selectBtn.interactable = canSelect;
    }

    private void OnEnable()
    {
        if (_selectBtn != null)
        {
            // Đơn giản hóa listener, chỉ gọi một hàm
            _selectBtn.onClick.AddListener(HandleClick);
        }
    }

    private void OnDisable()
    {
        // Hủy đăng ký listener của button
        if (_selectBtn != null)
        {
            _selectBtn.onClick.RemoveAllListeners();
        }
    }

    // Hàm được gọi khi button được nhấn
    private void HandleClick()
    {
        OnCardClicked?.Invoke(); 
        Debug.Log($"{gameObject} + {gameObject.name} + HandleClick ");
        Card.IsSelected = !Card.IsSelected; 
        IsSelected = Card.IsSelected;

        // 2. Cập nhật giao diện dựa trên trạng thái mới
        float targetY = IsSelected ? 50f : 0f;
        transform.DOLocalMoveY(targetY, 0.2f).SetEase(Ease.OutQuad);
    }
}