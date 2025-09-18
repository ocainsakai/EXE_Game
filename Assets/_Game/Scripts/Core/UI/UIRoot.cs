using UnityEngine;

public class UIRoot : MonoBehaviour
{
    [SerializeField] private ScreenManager screenManager;
    [SerializeField] private PopupManager popupManager;
    [SerializeField] private OverlayManager overlayManager;

    public ScreenManager Screens => screenManager;
    public PopupManager Popups => popupManager;
    public OverlayManager Overlays => overlayManager;
}
