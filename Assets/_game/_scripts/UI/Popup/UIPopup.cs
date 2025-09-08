using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Base class for all popups (simple version, no heavy animation).
/// </summary>
[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(GraphicRaycaster))]
public abstract class UIPopup : UIBase
{
    public object Parameter { get; protected set; }

    public UIContainer Container;
    public UIContainer Overlay;

    public VisibilityState State = VisibilityState.Hidden;

    private Canvas _canvas;
    private GraphicRaycaster _raycaster;
    private CanvasGroup _canvasGroup;

    public Canvas Canvas => _canvas ??= GetComponent<Canvas>();
    public GraphicRaycaster Raycaster => _raycaster ??= GetComponent<GraphicRaycaster>();
    public CanvasGroup CanvasGroup => _canvasGroup ??= gameObject.GetComponent<CanvasGroup>() ?? gameObject.AddComponent<CanvasGroup>();

    /// <summary>
    /// Called once when popup is created by Controller.
    /// </summary>
    public virtual void Initialize(object parameter = null)
    {
        Parameter = parameter;

        State = VisibilityState.Hidden;

        Overlay?.Disable();
        Container?.Disable();

        OnInit();
    }
    /// <summary>
    /// Show popup (with optional parameter).
    /// </summary>
    public override void Show(object data = null, Action<UIBase> onClosed = null)
    {
        base.Show(data, onClosed);
        if (data != null)
            Parameter = data;

        Canvas.enabled = true;
        Raycaster.enabled = true;

        Container?.Enable();

        State = VisibilityState.Shown;

        OnShowing();
        OnShown();
    }
    /// <summary>
    /// Hide popup (optionally destroy).
    /// </summary>
    public override void Hide()
    {
        base.Hide();
        if (State != VisibilityState.Shown) return;

        OnHiding();

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
