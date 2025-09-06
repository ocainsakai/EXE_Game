using UnityEngine;

public class UIPopupController : MonoBehaviour
{
    public UIPopupName PopupName;

    [Header("Behavior Settings")]
    public AfterHideBehaviour AfterHideBehaviour = AfterHideBehaviour.Destroy;
    public bool DeactiveGameObjectWhenHide = true;
    public bool ShowOverlay = true;

    private UIPopupManager _manager;
    private UIPopup _popup;

    public UIPopup Popup => _popup;

    public void Initialize(UIPopupManager manager)
    {
        _manager = manager;
    }

    public void Show(object param = null)
    {
        // Nếu popup đã cache sẵn
        if (_popup != null)
        {
            _popup.Show(param);
            return;
        }

        // Nếu chưa có thì nhờ Manager tạo
        _popup = _manager.CreatePopup(PopupName);
        if (_popup == null)
        {
            Debug.LogError($"[UIPopupController] Cannot create popup {PopupName}");
            return;
        }

        _popup.Initialize(this, param);
        _popup.Show(param);
    }

    public void Hide(bool instant = false)
    {
        if (_popup == null) return;
        _popup.Hide(instant);
    }

    public void DoDestroy()
    {
        if (_popup != null)
        {
            Destroy(_popup.gameObject);
            _popup = null;
        }
    }
}

public enum AfterHideBehaviour
{
    Disable,
    Destroy
}