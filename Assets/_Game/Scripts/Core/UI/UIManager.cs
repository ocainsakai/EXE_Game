using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityUtils;

public class UIManager : Singleton<UIManager>
{
    private Dictionary<Type, UIBase> uiRegistry = new Dictionary<Type, UIBase>();

    public T GetType<T>() where T : UIBase
    {
        if (uiRegistry.TryGetValue(typeof(T), out var ui))
        {
            return ui as T;
        }
        else
        {
            Debug.LogError($"UI {typeof(T)} not registered!");
            return null;
        }
    }
    public T Show<T>(object data = null, Action<T> onClosed = null) where T : UIBase
    {
        T ui = GetType<T>();
        if (ui != null)
        {
            ui.Show(data, (u) => onClosed?.Invoke(u as T));
            return ui;

        }
        return null;
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
