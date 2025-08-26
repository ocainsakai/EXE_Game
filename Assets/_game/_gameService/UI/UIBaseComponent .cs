using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class UIBaseComponent : MonoBehaviour
{
    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;

    public RectTransform RectTransform
    {
        get
        {
            if (_rectTransform == null) _rectTransform = GetComponent<RectTransform>();
            return _rectTransform;
        }
    }

    private CanvasGroup CanvasGroup
    {
        get
        {
            if (_canvasGroup == null) RectTransform.TryGetComponent(out _canvasGroup);
            return _canvasGroup;
        }
    }

    public int OrderInParent
    {
        get => transform.GetSiblingIndex();
        set => transform.SetSiblingIndex(Mathf.Clamp(value, 0, transform.parent != null ? transform.parent.childCount - 1 : 0));
    }

    [HideInInspector] public Vector3 StartPosition = Vector3.zero;
    [HideInInspector] public Vector3 StartRotation = Vector3.zero;
    [HideInInspector] public Vector3 StartScale = Vector3.one;
    [HideInInspector] public float StartAlpha = 1f;

    public virtual void Awake()
    {
        UpdateStartValues();
    }

    public virtual void ResetToStartValues()
    {
        SetCanvasGroupState(); // defaults
        ResetPosition();
        ResetRotation();
        ResetScale();
        ResetAlpha();
    }

    public virtual void ResetPosition() { RectTransform.anchoredPosition3D = StartPosition; }
    public virtual void ResetRotation() { RectTransform.localEulerAngles = StartRotation; }
    public virtual void ResetScale() { RectTransform.localScale = StartScale; }

    public virtual void ResetAlpha()
    {
        if (CanvasGroup != null) CanvasGroup.alpha = StartAlpha;
    }

    public void SetCanvasGroupState(bool interactable = true, bool blocksRaycasts = true)
    {
        if (CanvasGroup == null) return;
        CanvasGroup.interactable = interactable;
        CanvasGroup.blocksRaycasts = blocksRaycasts;
    }

    public virtual void UpdateStartValues()
    {
        UpdateStartPosition();
        UpdateStartRotation();
        UpdateStartScale();
        UpdateStartAlpha();
    }

    public virtual void UpdateStartPosition() { StartPosition = RectTransform.anchoredPosition3D; }
    public virtual void UpdateStartRotation() { StartRotation = RectTransform.localEulerAngles; }
    public virtual void UpdateStartScale() { StartScale = RectTransform.localScale; }
    public virtual void UpdateStartAlpha() { StartAlpha = CanvasGroup == null ? 1f : CanvasGroup.alpha; }

    public void BringForward() => OrderInParent = OrderInParent + 1;
    public void BringBackward() => OrderInParent = OrderInParent - 1;
    public void MoveToTop() => transform.SetAsLastSibling();
    public void MoveToBack() => transform.SetAsFirstSibling();

    public virtual bool OnBackClick()
    {
        Debug.Log("OnBackClick " + name);
        return true;
    }

    [ContextMenu("Update Start Values (Snapshot)")]
    private void SnapshotStartValues() => UpdateStartValues();
}
