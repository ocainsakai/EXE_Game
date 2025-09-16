using System;
using UnityEngine;

[Serializable]
public class UIContainer
{
    public RectTransform RectTransform;

    public void Enable()
    {
        if (RectTransform != null) RectTransform.gameObject.SetActive(true);
    }

    public void Disable()
    {
        if (RectTransform != null) RectTransform.gameObject.SetActive(false);
    }
}