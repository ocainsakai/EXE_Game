using System;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour  
{
    private Dictionary<Type, UIBase> uiRegistry = new Dictionary<Type, UIBase>();
    public T Show<T>(object data = null, Action<T> onClosed = null) where T : UIBase
    {
        if (uiRegistry.TryGetValue(typeof(T), out var ui))
        {
            var casted = ui as T;
            casted.Show(data, (u) => onClosed?.Invoke(casted));
            return casted;
        }
        else
        {
            Debug.LogError($"UI {typeof(T)} not registered!");
            return null;
        }
    }

    public void Hide<T>() where T : UIBase
    {
        if (uiRegistry.TryGetValue(typeof(T), out var ui))
        {
            ui.Hide();
        }
    }

    public void Register(UIBase ui)
    {
        var type = ui.GetType();
        if (!uiRegistry.ContainsKey(type))
        {
            uiRegistry[type] = ui;
            ui.gameObject.SetActive(false);
        }
    }
}
