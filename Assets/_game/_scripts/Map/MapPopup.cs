using System;
using UnityEngine;
using UnityEngine.UI;

public class MapPopup : MonoBehaviour
{
    [Header("UI Buttons")]
    [SerializeField] private Button action;
    [SerializeField] private Button close;

    private Action<UIBase> onClose;   // callback khi popup đóng
    private object parameter;         // data truyền vào popup

    private void Awake()
    {
        if (close != null)
        {
            close.onClick.RemoveAllListeners();
            close.onClick.AddListener(Hide);
        }
    }

    /// <summary>
    /// Hiển thị popup.
    /// </summary>
    public void Show(object data = null, Action<UIBase> onClose = null, Action actionCallback = null)
    {
        this.parameter = data;
        this.onClose = onClose;

        if (action != null)
        {
            action.onClick.RemoveAllListeners();

            if (actionCallback != null)
            {
                action.onClick.AddListener(() =>
                {
                    actionCallback.Invoke();
                    Hide(); // ẩn popup sau khi action chạy
                });
                action.gameObject.SetActive(true);
            }
            else
            {
                action.gameObject.SetActive(false);
            }
        }

        gameObject.SetActive(true); // bật popup
        OnShow();
    }

    /// <summary>
    /// Ẩn popup.
    /// </summary>
    public void Hide()
    {
        gameObject.SetActive(false); // tắt popup
        OnHide();

        // gọi callback nếu có
        onClose?.Invoke(null);
        onClose = null;
    }

    /// <summary>
    /// Gọi khi popup hiển thị (override nếu cần).
    /// </summary>
    protected virtual void OnShow()
    {
        // override trong class con (nếu có)
    }

    /// <summary>
    /// Gọi khi popup ẩn (override nếu cần).
    /// </summary>
    protected virtual void OnHide()
    {
        // override trong class con (nếu có)
    }
}
