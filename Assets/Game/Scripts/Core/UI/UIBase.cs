using System;
using UnityEngine;

public abstract class UIBase : MonoBehaviour
{
    protected Action<UIBase> onClosed;

    public virtual void Show(object data = null, Action<UIBase> onClosed = null) {

        gameObject.SetActive(true);
        this.onClosed = onClosed;
    }
    public virtual void Hide() {
        gameObject.SetActive(false);
        onClosed?.Invoke(this);
        onClosed = null;
    }
    public virtual void OnShowCompleted() { }
    public virtual void OnHideCompleted() { }
}
