using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Base class for all popups (simple version, no heavy animation).
/// </summary>
[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(GraphicRaycaster))]
public abstract class UIPopup : MonoBehaviour
{
    public UIPopupController Controller { get; private set; }
    public UIPopupName? PopupName => Controller?.PopupName;
    public object Parameter { get; protected set; }

    public UIContainer Container;
    public UIContainer Overlay;

    public VisibilityState State { get; private set; } = VisibilityState.Hidden;

    private Canvas _canvas;
    private GraphicRaycaster _raycaster;
    private CanvasGroup _canvasGroup;

    public Canvas Canvas => _canvas ??= GetComponent<Canvas>();
    public GraphicRaycaster Raycaster => _raycaster ??= GetComponent<GraphicRaycaster>();
    public CanvasGroup CanvasGroup => _canvasGroup ??= gameObject.GetComponent<CanvasGroup>() ?? gameObject.AddComponent<CanvasGroup>();

    /// <summary>
    /// Called once when popup is created by Controller.
    /// </summary>
    public virtual void Initialize(UIPopupController controller, object parameter = null)
    {
        Controller = controller;
        Parameter = parameter;

        State = VisibilityState.Hidden;

        Overlay?.Disable();
        Container?.Disable();

        OnInit();
    }

    /// <summary>
    /// Show popup (with optional parameter).
    /// </summary>
    public virtual void Show(object parameter = null)
    {
        if (parameter != null)
            Parameter = parameter;

        gameObject.SetActive(true);
        Canvas.enabled = true;
        Raycaster.enabled = true;

        Container?.Enable();
        if (Controller?.ShowOverlay ?? false) Overlay?.Enable();

        State = VisibilityState.Shown;

        OnShowing();
        OnShown();
    }

    /// <summary>
    /// Hide popup (optionally destroy).
    /// </summary>
    public virtual void Hide(bool instant = false)
    {
        if (State != VisibilityState.Shown) return;

        OnHiding();

        if (Controller?.AfterHideBehaviour == AfterHideBehaviour.Destroy)
        {
            Destroy(gameObject);
        }
        else
        {
            if (Controller?.DeactiveGameObjectWhenHide ?? false)
                gameObject.SetActive(false);
            else
            {
                Canvas.enabled = false;
                Raycaster.enabled = false;
            }
        }

        State = VisibilityState.Hidden;
        OnHidden();
    }

    // ====== Hooks for subclasses ======
    protected virtual void OnInit() { }
    protected virtual void OnShowing() { }
    protected virtual void OnShown() { }
    protected virtual void OnHiding() { }
    protected virtual void OnHidden() { }
}

public enum VisibilityState
{
    Hidden,
    Shown
}
