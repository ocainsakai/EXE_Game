using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static class DefaultPopupFactory
{
    public static UIPopup Generate(UIPopupName name, Transform parent)
    {
        // Root object
        var go = new GameObject($"{name}_Default", typeof(RectTransform), typeof(CanvasGroup), typeof(Canvas), typeof(GraphicRaycaster));
        go.transform.SetParent(parent, false);

        var rect = go.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.sizeDelta = new Vector2(500, 300);

        // Add Popup script (fallback = MessageBox)
        var popup = go.AddComponent<UIMessageBox>();

        // Overlay (đen mờ)
        GameObject overlay = new GameObject("Overlay", typeof(RectTransform), typeof(Image));
        overlay.transform.SetParent(go.transform, false);
        var overlayRect = overlay.GetComponent<RectTransform>();
        overlayRect.anchorMin = Vector2.zero;
        overlayRect.anchorMax = Vector2.one;
        overlayRect.offsetMin = overlayRect.offsetMax = Vector2.zero;
        overlay.GetComponent<Image>().color = new Color(0, 0, 0, 0.6f); // đen mờ
        popup.Overlay = new UIContainer { RectTransform = overlayRect };

        // Container (xám nhạt, bo góc)
        GameObject container = new GameObject("Container", typeof(RectTransform), typeof(Image));
        container.transform.SetParent(go.transform, false);
        var contRect = container.GetComponent<RectTransform>();
        contRect.anchorMin = new Vector2(0.5f, 0.5f);
        contRect.anchorMax = new Vector2(0.5f, 0.5f);
        contRect.pivot = new Vector2(0.5f, 0.5f);
        contRect.sizeDelta = new Vector2(400, 200);

        var contImg = container.GetComponent<Image>();
        contImg.color = new Color(0.9f, 0.9f, 0.9f, 1f); // xám nhạt
        contImg.raycastTarget = true;
        popup.Container = new UIContainer { RectTransform = contRect };

        // Message text (đen, center)
        var msgObj = new GameObject("Message", typeof(TextMeshProUGUI));
        msgObj.transform.SetParent(container.transform, false);
        var msgText = msgObj.GetComponent<TextMeshProUGUI>();
        msgText.alignment = TextAlignmentOptions.Center;
        msgText.color = Color.black;
        msgText.fontSize = 24;
        msgText.text = $"[Default {name}]";
        msgText.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        msgText.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        msgText.rectTransform.pivot = new Vector2(0.5f, 0.5f);
        msgText.rectTransform.sizeDelta = new Vector2(350, 100);

        // Ok Button (nền xanh, chữ trắng)
        var btnObj = new GameObject("OkButton", typeof(RectTransform), typeof(Image), typeof(Button));
        btnObj.transform.SetParent(container.transform, false);
        var btnRect = btnObj.GetComponent<RectTransform>();
        btnRect.anchorMin = new Vector2(0.5f, 0);
        btnRect.anchorMax = new Vector2(0.5f, 0);
        btnRect.pivot = new Vector2(0.5f, 0);
        btnRect.anchoredPosition = new Vector2(0, 20);
        btnRect.sizeDelta = new Vector2(120, 40);

        var btnImg = btnObj.GetComponent<Image>();
        btnImg.color = new Color(0.2f, 0.5f, 0.9f, 1f); // xanh dương nhạt

        var btn = btnObj.GetComponent<Button>();
        btn.onClick.AddListener(() => popup.Hide());

        var btnTextObj = new GameObject("Text", typeof(TextMeshProUGUI));
        btnTextObj.transform.SetParent(btnObj.transform, false);
        var btnText = btnTextObj.GetComponent<TextMeshProUGUI>();
        btnText.alignment = TextAlignmentOptions.Center;
        btnText.color = Color.white;
        btnText.fontSize = 20;
        btnText.text = "OK";
        btnText.rectTransform.sizeDelta = new Vector2(120, 40);

        return popup;
    }
}
