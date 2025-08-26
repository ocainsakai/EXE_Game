using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(GraphicRaycaster))]
[RequireComponent(typeof(CanvasGroup))] // ensure CanvasGroup exists
[DisallowMultipleComponent]
public abstract class UIPopup : UIBaseComponent
{
    //#region Public Vars
    //public UIContainer Container;
    //public UIContainer Overlay;
    //#endregion

    //#region Private Vars
    //private bool _initialized;
    //private Canvas _canvas;
    //private GraphicRaycaster _graphicRaycaster;
    //private CanvasGroup _canvasGroup;
    //private Coroutine _showCoroutine;
    //private Coroutine _hideCoroutine;
    //#endregion

    //#region Properties
    //public VisibilityState VisibilityState { get; private set; }
    //public UIPopupController Controller { get; private set; }
    //public object Parameter { get; protected set; }

    //public Canvas Canvas
    //{
    //    get
    //    {
    //        if (_canvas == null) _canvas = GetComponent<Canvas>();
    //        return _canvas;
    //    }
    //}

    //public GraphicRaycaster GraphicRaycaster
    //{
    //    get
    //    {
    //        if (_graphicRaycaster == null) _graphicRaycaster = GetComponent<GraphicRaycaster>();
    //        return _graphicRaycaster;
    //    }
    //}

    //public CanvasGroup CanvasGroup
    //{
    //    get
    //    {
    //        if (_canvasGroup == null) TryGetComponent(out _canvasGroup);
    //        return _canvasGroup;
    //    }
    //}
    //#endregion

    //#region Unity Methods
    //private void OnValidate()
    //{
    //    // Editor helper: try to auto-assign Container and Overlay RectTransforms if missing
    //    if (Container != null && Container.RectTransform == null)
    //        Container.RectTransform = transform.Find("Container")?.GetComponent<RectTransform>();
    //    if (Overlay != null && Overlay.RectTransform == null)
    //        Overlay.RectTransform = transform.Find("Overlay")?.GetComponent<RectTransform>();
    //}

    //public override void Awake()
    //{
    //    base.Awake();

    //    // Ensure Container/Overlay references exist at runtime too (fallback if OnValidate not run)
    //    if (Container == null)
    //    {
    //        var c = transform.Find("Container");
    //        if (c != null)
    //        {
    //            Container = new UIContainer { RectTransform = c.GetComponent<RectTransform>() };
    //        }
    //    }
    //    else if (Container.RectTransform == null)
    //    {
    //        Container.RectTransform = transform.Find("Container")?.GetComponent<RectTransform>();
    //    }

    //    if (Overlay == null)
    //    {
    //        var o = transform.Find("Overlay");
    //        if (o != null)
    //        {
    //            Overlay = new UIContainer { RectTransform = o.GetComponent<RectTransform>() };
    //        }
    //    }
    //    else if (Overlay.RectTransform == null)
    //    {
    //        Overlay.RectTransform = transform.Find("Overlay")?.GetComponent<RectTransform>();
    //    }

    //    _initialized = false;
    //}
    //#endregion

    //#region Public Methods

    //public void Initialize(UIPopupController controller, object p)
    //{
    //    if (_initialized) return;

    //    Controller = controller ?? throw new ArgumentNullException(nameof(controller));

    //    if (p != null)
    //        Parameter = p;

    //    VisibilityState = VisibilityState.Hidden;

    //    // Guard Container/Overlay existence
    //    if (Container == null || Container.RectTransform == null)
    //    {
    //        Debug.LogError($"[{name}] Container not assigned or missing RectTransform.", this);
    //    }
    //    if (Overlay == null || Overlay.RectTransform == null)
    //    {
    //        Debug.LogWarning($"[{name}] Overlay not assigned or missing RectTransform.", this);
    //    }

    //    // start hidden
    //    if (Overlay != null) Overlay.Disable();
    //    if (Container != null) Container.Disable();

    //    // Setup overlay click to close (only if controller allows)
    //    if (Controller.CloseByClickOutside && Overlay != null && Overlay.RectTransform != null)
    //    {
    //        var b = Overlay.RectTransform.GetComponent<Button>();
    //        if (!b)
    //        {
    //            b = Overlay.RectTransform.gameObject.AddComponent<Button>();
    //            b.transition = Selectable.Transition.None;
    //        }

    //        b.onClick.RemoveAllListeners();
    //        b.onClick.AddListener(() =>
    //        {
    //            Base.UIManager.DebugLog("Close by click Overlay", this);
    //            Hide();
    //        });
    //    }

    //    OnInit();

    //    _initialized = true;
    //}

    //public void Show(object p = null)
    //{
    //    if (Controller == null)
    //    {
    //        Debug.LogError($"[{name}] Show() called but Controller is null. Did you Initialize()?", this);
    //        return;
    //    }

    //    Base.UIManager.DebugLog("Show " + Controller.PopupName, this);

    //    if (p != null) Parameter = p;

    //    StopHide();

    //    bool instantAction = Controller.ShowBehavior.InstantAnimation;

    //    if (!Controller.ShowBehavior.Animation.Enabled && !instantAction)
    //    {
    //        Debug.LogError($"You are trying to SHOW the ({name}) UIPopup, but you did not enable any SHOW animations.");
    //        return;
    //    }

    //    if (VisibilityState == VisibilityState.Showing)
    //    {
    //        Base.UIManager.DebugLog("Popup is showing, pls wait " + Controller.PopupName, this);
    //        return;
    //    }

    //    // update order
    //    if (Controller.AlwaysOnTop)
    //    {
    //        MoveToTop();
    //    }
    //    else if (Controller.PopupManager.GetHighestAlwaysOnTopPopupOrder() > 0)
    //    {
    //        OrderInParent = Controller.PopupManager.GetLowestAlwaysOnTopPopupOrder() - 1;
    //    }
    //    else
    //    {
    //        OrderInParent = Controller.PopupManager.GetActiveTopPopupOrder() + 1;
    //    }

    //    if (VisibilityState == VisibilityState.Shown)
    //    {
    //        Base.UIManager.DebugLog("Popup was shown " + Controller.PopupName, this);
    //        Controller.PopupManager.AddVisiblePopup(this);
    //        try { OnShown(); } catch (Exception e) { Debug.LogError(e); }
    //        return;
    //    }

    //    _showCoroutine = StartCoroutine(ShowEnumerator(instantAction));
    //}

    //private IEnumerator ShowEnumerator(bool instantAction)
    //{
    //    yield return null; // ensure one frame skip to avoid race

    //    // Stop any ongoing show animations
    //    UIAnimator.StopAnimations(Container.RectTransform, Controller.ShowBehavior.Animation.AnimationType);

    //    // Enable objects/components
    //    gameObject.SetActive(true);
    //    Canvas.enabled = true;
    //    GraphicRaycaster.enabled = true;
    //    Container?.Enable();
    //    if (Controller.ShowOverlay) Overlay?.Enable(); else Overlay?.Disable();

    //    // Move
    //    Vector3 moveFrom = UIAnimator.GetAnimationMoveFrom(Container.RectTransform, Controller.ShowBehavior.Animation, Vector3.zero);
    //    Vector3 moveTo = UIAnimator.GetAnimationMoveTo(Container.RectTransform, Controller.ShowBehavior.Animation, Vector3.zero);
    //    if (!Controller.ShowBehavior.Animation.Move.Enabled || instantAction) Container.ResetPosition();
    //    UIAnimator.Move(Container.RectTransform, Controller.ShowBehavior.Animation, moveFrom, moveTo, instantAction);

    //    // Rotate
    //    Vector3 rotateFrom = UIAnimator.GetAnimationRotateFrom(Controller.ShowBehavior.Animation, Vector3.zero);
    //    Vector3 rotateTo = UIAnimator.GetAnimationRotateTo(Controller.ShowBehavior.Animation, Vector3.zero);
    //    if (!Controller.ShowBehavior.Animation.Rotate.Enabled || instantAction) Container.ResetRotation();
    //    UIAnimator.Rotate(Container.RectTransform, Controller.ShowBehavior.Animation, rotateFrom, rotateTo, instantAction);

    //    // Scale
    //    Vector3 scaleFrom = UIAnimator.GetAnimationScaleFrom(Controller.ShowBehavior.Animation, Vector3.one);
    //    Vector3 scaleTo = UIAnimator.GetAnimationScaleTo(Controller.ShowBehavior.Animation, Vector3.one);
    //    if (!Controller.ShowBehavior.Animation.Scale.Enabled || instantAction) Container.ResetScale();
    //    UIAnimator.Scale(Container.RectTransform, Controller.ShowBehavior.Animation, scaleFrom, scaleTo, instantAction);

    //    // Fade
    //    float fadeFrom = UIAnimator.GetAnimationFadeFrom(Controller.ShowBehavior.Animation, 1f);
    //    float fadeTo = UIAnimator.GetAnimationFadeTo(Controller.ShowBehavior.Animation, 1f);
    //    if (!Controller.ShowBehavior.Animation.Fade.Enabled || instantAction) Container.ResetAlpha();
    //    UIAnimator.Fade(Container.RectTransform, Controller.ShowBehavior.Animation, fadeFrom, fadeTo, instantAction);

    //    // Fade content (CanvasGroup)
    //    if (Controller.FadeContent)
    //    {
    //        if (CanvasGroup != null)
    //            CanvasGroup.DOFade(1f, Controller.ShowBehavior.Animation.TotalDuration);
    //        else
    //            Debug.LogWarning($"[{name}] Controller requested FadeContent but CanvasGroup missing.", this);
    //    }
    //    else
    //    {
    //        if (CanvasGroup != null) CanvasGroup.alpha = 1f;
    //    }

    //    VisibilityState = VisibilityState.Showing;
    //    OnShowing();
    //    Controller.ShowBehavior.OnStart.Invoke(gameObject, !instantAction, !instantAction);

    //    if (!instantAction)
    //    {
    //        yield return new WaitForSecondsRealtime(Mathf.Max(0f, Controller.ShowBehavior.Animation.TotalDuration));
    //    }

    //    Controller.ShowBehavior.OnFinished.Invoke(gameObject, !instantAction, !instantAction);

    //    _showCoroutine = null;
    //    OnShowAnimCompleted();
    //}

    //public virtual void Hide(bool instantHide = false)
    //{
    //    if (Controller == null)
    //    {
    //        Debug.LogError($"[{name}] Hide() called but Controller is null. Did you Initialize()?", this);
    //        return;
    //    }

    //    bool instantAction = instantHide || Controller.HideBehavior.InstantAnimation;

    //    StopShow();

    //    if (!Controller.HideBehavior.Animation.Enabled && !instantAction)
    //    {
    //        Debug.LogError($"You are trying to HIDE the ({name}) UIPopup, but you did not enable any HIDE animations.");
    //        return;
    //    }

    //    if (VisibilityState == VisibilityState.Hiding) return;

    //    _hideCoroutine = StartCoroutine(HideEnumerator(instantAction));
    //}

    //private IEnumerator HideEnumerator(bool instantAction)
    //{
    //    // Stop any show animations of this container
    //    UIAnimator.StopAnimations(Container.RectTransform, Controller.HideBehavior.Animation.AnimationType);

    //    // Move
    //    Vector3 moveFrom = UIAnimator.GetAnimationMoveFrom(Container.RectTransform, Controller.HideBehavior.Animation, Vector3.zero);
    //    Vector3 moveTo = UIAnimator.GetAnimationMoveTo(Container.RectTransform, Controller.HideBehavior.Animation, Vector3.zero);
    //    if (!Controller.HideBehavior.Animation.Move.Enabled || instantAction) Container.ResetPosition();
    //    UIAnimator.Move(Container.RectTransform, Controller.HideBehavior.Animation, moveFrom, moveTo, instantAction);

    //    // Rotate
    //    Vector3 rotateFrom = UIAnimator.GetAnimationRotateFrom(Controller.HideBehavior.Animation, Vector3.zero);
    //    Vector3 rotateTo = UIAnimator.GetAnimationRotateTo(Controller.HideBehavior.Animation, Vector3.zero);
    //    if (!Controller.HideBehavior.Animation.Rotate.Enabled || instantAction) Container.ResetRotation();
    //    UIAnimator.Rotate(Container.RectTransform, Controller.HideBehavior.Animation, rotateFrom, rotateTo, instantAction);

    //    // Scale
    //    Vector3 scaleFrom = UIAnimator.GetAnimationScaleFrom(Controller.HideBehavior.Animation, Vector3.one);
    //    Vector3 scaleTo = UIAnimator.GetAnimationScaleTo(Controller.HideBehavior.Animation, Vector3.one);
    //    if (!Controller.HideBehavior.Animation.Scale.Enabled || instantAction) Container.ResetScale();
    //    UIAnimator.Scale(Container.RectTransform, Controller.HideBehavior.Animation, scaleFrom, scaleTo, instantAction);

    //    // Fade
    //    float fadeFrom = UIAnimator.GetAnimationFadeFrom(Controller.HideBehavior.Animation, 1f);
    //    float fadeTo = UIAnimator.GetAnimationFadeTo(Controller.HideBehavior.Animation, 1f);
    //    if (!Controller.HideBehavior.Animation.Fade.Enabled || instantAction) Container.ResetAlpha();
    //    UIAnimator.Fade(Container.RectTransform, Controller.HideBehavior.Animation, fadeFrom, fadeTo, instantAction);

    //    // FIXED: use HideBehavior duration (was ShowBehavior before)
    //    if (Controller.FadeContent)
    //    {
    //        if (CanvasGroup != null)
    //            CanvasGroup.DOFade(0f, Controller.HideBehavior.Animation.TotalDuration);
    //        else
    //            Debug.LogWarning($"[{name}] Controller requested FadeContent but CanvasGroup missing.", this);
    //    }

    //    VisibilityState = VisibilityState.Hiding;
    //    OnHiding();
    //    Controller.HideBehavior.OnStart.Invoke(gameObject, !instantAction, !instantAction);

    //    if (!instantAction)
    //    {
    //        yield return new WaitForSecondsRealtime(Mathf.Max(0f, Controller.HideBehavior.Animation.TotalDuration + 0.05f));
    //    }

    //    Controller.HideBehavior.OnFinished.Invoke(gameObject, !instantAction, !instantAction);

    //    _hideCoroutine = null;
    //    OnTweenHideCompleted();
    //}

    //public override bool OnBackClick()
    //{
    //    base.OnBackClick();
    //    if (Controller != null && Controller.CloseByBackButton)
    //    {
    //        Hide();
    //        return true;
    //    }
    //    return false;
    //}
    //#endregion

    //#region Virtual Methods
    //protected virtual void OnInit() { Base.UIManager.DebugLog("OnInit", this); }
    //protected virtual void OnShown() { Base.UIManager.DebugLog("OnShown", this); }
    //protected virtual void OnHiding() { Base.UIManager.DebugLog("OnHiding", this); }
    //protected virtual void OnShowing() { Base.UIManager.DebugLog("OnShowing", this); }
    //protected virtual void OnHidden() { Base.UIManager.DebugLog("OnHidden", this); }
    //#endregion

    //#region Private Methods

    //void OnTweenHideCompleted()
    //{
    //    VisibilityState = VisibilityState.Hidden;
    //    if (Controller?.PopupManager != null)
    //    {
    //        Controller.PopupManager.RemoveVisiblePopup(this);
    //        Controller.PopupManager.RemoveHiddenFromVisiblePopups();
    //    }

    //    OnHidden();

    //    Overlay?.Disable();
    //    Container?.Disable();

    //    if (Controller != null && Controller.DeactiveGameObjectWhenHide)
    //        gameObject.SetActive(false);
    //    else
    //    {
    //        Canvas.enabled = false;
    //        GraphicRaycaster.enabled = false;
    //    }

    //    if (Controller != null && Controller.AfterHideBehaviour == AfterHideBehaviour.Destroy)
    //    {
    //        StopAllCoroutines();
    //        Destroy(gameObject);
    //    }
    //}

    //void OnShowAnimCompleted()
    //{
    //    try { OnShown(); } catch (Exception e) { Debug.LogError(e); }

    //    VisibilityState = VisibilityState.Shown;
    //    Controller?.PopupManager?.RemoveHiddenFromVisiblePopups();
    //    Controller?.PopupManager?.AddVisiblePopup(this);
    //}

    //private void StopHide()
    //{
    //    if (_hideCoroutine == null) return;

    //    Debug.Log($"[{name}] Stop Hide");

    //    StopCoroutine(_hideCoroutine);
    //    _hideCoroutine = null;

    //    UIAnimator.StopAnimations(Container.RectTransform, AnimationType.Hide);

    //    // force to final HIDE state
    //    Container?.ResetPosition();
    //    Container?.ResetRotation();
    //    Container?.ResetScale();
    //    Container?.ResetAlpha();
    //    if (Controller?.FadeContent == true && CanvasGroup != null) CanvasGroup.alpha = 0f;

    //    VisibilityState = VisibilityState.Hidden;
    //    Controller?.PopupManager?.RemoveVisiblePopup(this);
    //}

    //private void StopShow()
    //{
    //    if (_showCoroutine == null) return;

    //    Debug.Log($"[{name}] Stop Show");

    //    StopCoroutine(_showCoroutine);
    //    _showCoroutine = null;

    //    UIAnimator.StopAnimations(Container.RectTransform, AnimationType.Show);

    //    // force to final SHOW state
    //    Container?.ResetPosition();
    //    Container?.ResetRotation();
    //    Container?.ResetScale();
    //    if (Controller?.FadeContent == true && CanvasGroup != null) CanvasGroup.alpha = 1f;

    //    VisibilityState = VisibilityState.Shown;
    //    Controller?.PopupManager?.AddVisiblePopup(this);
    //}

    //#endregion
}
