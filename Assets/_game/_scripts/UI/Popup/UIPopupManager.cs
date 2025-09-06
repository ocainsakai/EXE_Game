using System.Collections.Generic;
using UnityEngine;

public class UIPopupManager : MonoBehaviour
{
    public RectTransform PopupsContainer;
    private Dictionary<UIPopupName, UIPopupController> controllers = new();

    void Awake()
    {
        // Scan toàn bộ controllers gắn trong scene
        foreach (var ctrl in GetComponentsInChildren<UIPopupController>(true))
        {
            ctrl.Initialize(this);
            controllers[ctrl.PopupName] = ctrl;
        }
    }

    public void ShowPopup(UIPopupName name, object param = null)
    {
        // Nếu đã có controller thì gọi luôn
        if (controllers.TryGetValue(name, out var ctrl))
        {
            ctrl.Show(param);
            return;
        }

        Debug.LogWarning($"[UIPopupManager] No controller found for {name}, creating default one...");

        // 1. Tạo GameObject controller
        GameObject ctrlObj = new GameObject($"{name}_Controller", typeof(UIPopupController));
        ctrlObj.transform.SetParent(this.transform, false);

        ctrl = ctrlObj.GetComponent<UIPopupController>();
        ctrl.PopupName = name;
        ctrl.Initialize(this);

        // Đăng ký vào dictionary để lần sau gọi nhanh hơn
        controllers[name] = ctrl;

        // 2. Show popup (tự tạo bằng factory nếu chưa có prefab)
        ctrl.Show(param);
    }


    public UIPopup CreatePopup(UIPopupName name)
    {
        // 1. thử load prefab
        GameObject prefab = Resources.Load<GameObject>($"Prefabs/Popups/{name}");
        GameObject go;
        if (prefab)
        {
            go = Instantiate(prefab, PopupsContainer);
        }
        else
        {
            // 2. fallback -> generate default UI
            go = DefaultPopupFactory.Generate(name, PopupsContainer).gameObject;
        }

        var popup = go.GetComponent<UIPopup>();
        if (!popup)
        {
            Debug.LogError($"Popup {name} has no UIPopup script!");
            Destroy(go);
            return null;
        }
        return popup;
    }
}
