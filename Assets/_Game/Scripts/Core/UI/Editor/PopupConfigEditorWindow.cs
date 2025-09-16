using UnityEngine;
using UnityEditor;
using TMPro;

public class PopupConfigEditorWindow : EditorWindow
{
    // Các field để chỉnh trong editor
    private Vector2 containerSize = new Vector2(400, 200);
    private Color containerColor = Color.white;

    private Color overlayColor = new Color(0, 0, 0, 0.6f);

    private int fontSize = 24;
    private Color textColor = Color.black;
    private TextAlignmentOptions textAlignment = TextAlignmentOptions.Center;

    private Vector2 buttonSize = new Vector2(120, 40);
    private Color buttonBackgroundColor = new Color(0.2f, 0.5f, 0.9f);
    private Color buttonTextColor = Color.white;
    private int buttonFontSize = 20;

    private bool hasCancelButton = false;

    private string configName = "NewPopupConfig";
    private string folderPath = "Assets/Resources/PopupConfigs"; // default

    [MenuItem("Tools/UI/Popup Config Generator")]
    public static void ShowWindow()
    {
        GetWindow<PopupConfigEditorWindow>("Popup Config Generator");
    }

    void OnGUI()
    {
        GUILayout.Label("Popup Config Generator", EditorStyles.boldLabel);

        configName = EditorGUILayout.TextField("Config Name", configName);

        // Folder picker
        EditorGUILayout.BeginHorizontal();
        folderPath = EditorGUILayout.TextField("Save Folder", folderPath);
        if (GUILayout.Button("...", GUILayout.MaxWidth(30)))
        {
            string selected = EditorUtility.OpenFolderPanel("Select Folder", folderPath, "");
            if (!string.IsNullOrEmpty(selected))
            {
                if (selected.StartsWith(Application.dataPath))
                {
                    folderPath = "Assets" + selected.Substring(Application.dataPath.Length);
                }
                else
                {
                    EditorUtility.DisplayDialog("Invalid Folder", "Please select a folder inside Assets/", "OK");
                }
            }
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();
        GUILayout.Label("Container", EditorStyles.boldLabel);
        containerSize = EditorGUILayout.Vector2Field("Size", containerSize);
        containerColor = EditorGUILayout.ColorField("Background Color", containerColor);

        EditorGUILayout.Space();
        GUILayout.Label("Overlay", EditorStyles.boldLabel);
        overlayColor = EditorGUILayout.ColorField("Overlay Color", overlayColor);

        EditorGUILayout.Space();
        GUILayout.Label("Text", EditorStyles.boldLabel);
        fontSize = EditorGUILayout.IntField("Font Size", fontSize);
        textColor = EditorGUILayout.ColorField("Text Color", textColor);
        textAlignment = (TextAlignmentOptions)EditorGUILayout.EnumPopup("Alignment", textAlignment);

        EditorGUILayout.Space();
        GUILayout.Label("Button", EditorStyles.boldLabel);
        buttonSize = EditorGUILayout.Vector2Field("Button Size", buttonSize);
        buttonBackgroundColor = EditorGUILayout.ColorField("Background Color", buttonBackgroundColor);
        buttonTextColor = EditorGUILayout.ColorField("Text Color", buttonTextColor);
        buttonFontSize = EditorGUILayout.IntField("Font Size", buttonFontSize);
        hasCancelButton = EditorGUILayout.Toggle("Has Cancel Button", hasCancelButton);

        EditorGUILayout.Space();
        if (GUILayout.Button("Generate Config Asset", GUILayout.Height(30)))
        {
            GenerateConfig(folderPath, configName);
        }
    }

    private void GenerateConfig(string folder, string name)
    {
        if (!AssetDatabase.IsValidFolder(folder))
        {
            EditorUtility.DisplayDialog("Error", $"Folder {folder} is not valid!", "OK");
            return;
        }

        var config = ScriptableObject.CreateInstance<PopupConfig>();

        config.size = containerSize;
        config.backgroundColor = containerColor;
        config.overlayColor = overlayColor;
        config.fontSize = fontSize;
        config.textColor = textColor;
        config.textAlignment = textAlignment;
        config.buttonSize = buttonSize;
        config.buttonBackgroundColor = buttonBackgroundColor;
        config.buttonTextColor = buttonTextColor;
        config.buttonFontSize = buttonFontSize;
        config.hasCancelButton = hasCancelButton;

        string path = $"{folder}/{name}.asset";
        AssetDatabase.CreateAsset(config, path);
        AssetDatabase.SaveAssets();

        EditorUtility.DisplayDialog("Popup Config Generator", $"Config saved to {path}", "OK");
    }
}
