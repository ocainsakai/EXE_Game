// CardEntry.cs - Refactored
using DG.Tweening;
using _Game.Addons.Deck.Scripts;
using _Game.Addons.Deck.Scripts.Card;
using UnityEngine;
using UnityEngine.UI;

public class CardEntry : MonoBehaviour
{
    [SerializeField] private Image art;
    [SerializeField] private Button selectBtn;
    
    public SerializableGuid CardID => CardRuntime.CardID;
    public CardRuntime CardRuntime { get; private set; }
    private Room _room;

    public void SetRoom(Room room)
    {
        _room = room;
    }
    public void Setup(CardRuntime cardRuntime)
    {
        this.CardRuntime = cardRuntime;
        
        art.sprite = cardRuntime.Art;
        
        // Luôn đồng bộ trạng thái UI với trạng thái logic khi setup
        UpdateVisuals(false); // Cập nhật ngay lập tức, không có animation
    }

    public void SetInteractable(bool isInteractable)
    {
        selectBtn.interactable = isInteractable;
    }

    private void OnEnable()
    {
        selectBtn.onClick.AddListener(HandleClick);
    }

    private void OnDisable()
    {
        selectBtn.onClick.RemoveListener(HandleClick);
    }

    private void HandleClick()
    {
        // Logic kiểm tra đã đúng từ trước: chỉ chặn chọn thêm, luôn cho phép bỏ chọn.
        if (!CardRuntime.IsSelected && !_room.CanSelectCard)
        {
            Debug.Log("Cannot select more cards!");
            return;
        }

        // Thay đổi trạng thái logic "nguồn"
        CardRuntime.IsSelected = !CardRuntime.IsSelected;

        // Cập nhật giao diện sau khi thay đổi logic
        UpdateVisuals(true);
    }

    // Hàm riêng để cập nhật giao diện, có thể gọi từ nhiều nơi
    public void UpdateVisuals(bool animated)
    {
        float targetY = CardRuntime.IsSelected ? 50f : 0f;
        if (animated)
        {
            transform.DOLocalMoveY(targetY, 0.2f).SetEase(Ease.OutQuad);
        }
        else
        {
            // Cập nhật ngay lập tức
            var pos = transform.localPosition;
            pos.y = targetY;
            transform.localPosition = pos;
        }
    }
}