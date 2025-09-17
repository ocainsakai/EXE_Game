using UnityEngine;
using UnityEditor;
using TMPro;

public class PopupConfigPreviewWindow : EditorWindow
{
    private PopupConfig config;
    private SerializedObject serializedConfig;
    private Vector2 scroll;

    private bool showContainer = true;
    private bool showOverlay = true;
    private bool showText = true;
    private bool showButton = true;
    private bool showPopupType = true;

    [MenuItem("Tools/UI/Popup Config Preview & Edit (Compact)")]
    public static void ShowWindow()
    {
        GetWindow<PopupConfigPreviewWindow>("Popup Config Editor");
    }

    private void OnGUI()
    {
        GUILayout.Label("Popup Config Preview & Edit", EditorStyles.boldLabel);

        var newConfig = (PopupConfig)EditorGUILayout.ObjectField("Popup Config", config, typeof(PopupConfig), false);
        if (newConfig != config)
        {
            config = newConfig;
            serializedConfig = config != null ? new SerializedObject(config) : null;
        }

        if (config == null)
        {
            EditorGUILayout.HelpBox("Please assign a PopupConfig to preview & edit.", MessageType.Info);
            return;
        }

        serializedConfig.Update();

        EditorGUILayout.Space();

        // Container
        showContainer = EditorGUILayout.Foldout(showContainer, "Container Settings", true);
        if (showContainer)
        {
            config.size = EditorGUILayout.Vector2Field("Size", config.size);
            config.backgroundColor = EditorGUILayout.ColorField("Background", config.backgroundColor);
        }

        // Overlay
        showOverlay = EditorGUILayout.Foldout(showOverlay, "Overlay Settings", true);
        if (showOverlay)
        {
            config.overlayColor = EditorGUILayout.ColorField("Overlay Color", config.overlayColor);
        }

        // Text
        showText = EditorGUILayout.Foldout(showText, "Text Settings", true);
        if (showText)
        {
            config.fontSize = EditorGUILayout.IntSlider("Font Size", config.fontSize, 10, 80);
            config.textColor = EditorGUILayout.ColorField("Text Color", config.textColor);
            config.textAlignment = (TextAlignmentOptions)EditorGUILayout.EnumPopup("Alignment", config.textAlignment);
        }

        // Button
        showButton = EditorGUILayout.Foldout(showButton, "Button Settings", true);
        if (showButton)
        {
            config.buttonSize = EditorGUILayout.Vector2Field("Size", config.buttonSize);
            config.buttonBackgroundColor = EditorGUILayout.ColorField("Background", config.buttonBackgroundColor);
            config.buttonTextColor = EditorGUILayout.ColorField("Text Color", config.buttonTextColor);
            config.buttonFontSize = EditorGUILayout.IntSlider("Font Size", config.buttonFontSize, 8, 40);
        }

        // Popup type
        showPopupType = EditorGUILayout.Foldout(showPopupType, "Popup Type", true);
        if (showPopupType)
        {
            config.hasCancelButton = EditorGUILayout.Toggle("Has Cancel Button", config.hasCancelButton);
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(config);
        }

        EditorGUILayout.Space();
        GUILayout.Label("Preview", EditorStyles.boldLabel);

        DrawPreview();
    }

    private void DrawPreview()
    {
        scroll = EditorGUILayout.BeginScrollView(scroll, GUILayout.Height(position.height / 2));

        float previewWidth = position.width - 40;
        float previewHeight = config.size.y + 120;

        Rect previewRect = GUILayoutUtility.GetRect(previewWidth, previewHeight);
        GUI.Box(previewRect, GUIContent.none);

        // Overlay
        EditorGUI.DrawRect(previewRect, config.overlayColor);

        // Container
        Rect containerRect = new Rect(
            previewRect.x + (previewRect.width - config.size.x) / 2,
            previewRect.y + (previewRect.height - config.size.y) / 2,
            config.size.x,
            config.size.y
        );
        EditorGUI.DrawRect(containerRect, config.backgroundColor);

        // Message Text
        Rect msgRect = new Rect(containerRect.x + 10, containerRect.y + 10, containerRect.width - 20, 50);
        GUIStyle textStyle = new GUIStyle(EditorStyles.label)
        {
            alignment = TextAnchor.MiddleCenter,
            fontSize = config.fontSize,
            normal = { textColor = config.textColor }
        };
        GUI.Label(msgRect, "[Preview Message]", textStyle);

        // OK Button
        Rect okBtnRect = new Rect(
            containerRect.x + containerRect.width / 2 - config.buttonSize.x - 5,
            containerRect.yMax - config.buttonSize.y - 10,
            config.buttonSize.x,
            config.buttonSize.y
        );
        EditorGUI.DrawRect(okBtnRect, config.buttonBackgroundColor);
        GUIStyle btnStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            alignment = TextAnchor.MiddleCenter,
            fontSize = config.buttonFontSize,
            normal = { textColor = config.buttonTextColor }
        };
        GUI.Label(okBtnRect, "OK", btnStyle);

        if (config.hasCancelButton)
        {
            Rect cancelBtnRect = new Rect(
                containerRect.x + containerRect.width / 2 + 5,
                containerRect.yMax - config.buttonSize.y - 10,
                config.buttonSize.x,
                config.buttonSize.y
            );
            EditorGUI.DrawRect(cancelBtnRect, config.buttonBackgroundColor * 0.8f);
            GUI.Label(cancelBtnRect, "Cancel", btnStyle);
        }

        EditorGUILayout.EndScrollView();
    }
}
