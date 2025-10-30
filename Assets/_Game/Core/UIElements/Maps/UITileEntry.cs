using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UITileEntry : MonoBehaviour
{
    [FormerlySerializedAs("_background")][SerializeField] Image background;
    [FormerlySerializedAs("_content")][SerializeField] Image content;
    [FormerlySerializedAs("_button")][SerializeField] Button button;

    public static UnityAction<Vector2Int> OnTileMapClicked;

    private Vector2Int _position;

    /// <summary>
    /// Hàm Awake được gọi một lần duy nhất khi Prefab được tạo.
    /// Chúng ta đăng ký listener ở đây để tránh rò rỉ bộ nhớ.
    /// </summary>
    void Awake()
    {
        // Xóa mọi listener cũ có thể có trong Prefab (để an toàn)
        button.onClick.RemoveAllListeners();

        // Chỉ thêm listener MỘT LẦN
        button.onClick.AddListener(HandleClick);
    }

    /// <summary>
    /// Hàm này được gọi khi nút được nhấp
    /// </summary>
    private void HandleClick()
    {
        // Gửi sự kiện static, truyền vị trí của ô này
        OnTileMapClicked?.Invoke(_position);
    }

    /// <summary>
    /// Cập nhật dữ liệu hình ảnh cho ô
    /// </summary>
    public void SetData(Vector2Int position, Sprite icon, Color bgColor)
    {
        _position = position; // Cập nhật vị trí

        // Sửa lỗi "mất icon": Luôn bật/tắt GameObject
        if (icon != null)
        {
            content.gameObject.SetActive(true);
            content.sprite = icon;
        }
        else
        {
            content.gameObject.SetActive(false);
        }

        // Đặt màu nền
        background.color = bgColor;

        // KHÔNG còn AddListener ở đây nữa
    }

    /// <Ghi chú>
    /// Hàm OnDestroy vẫn tốt, nó dọn dẹp khi đối tượng bị hủy.
    /// Nhưng vì chúng ta dùng AddListener trong Awake, nên sửa lại cho đúng.
    /// </Ghi chú>
    void OnDestroy()
    {
        // Chỉ xóa listener mà chúng ta đã thêm
        button.onClick.RemoveListener(HandleClick);
    }
}