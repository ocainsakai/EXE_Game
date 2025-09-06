using UnityEngine;
using UnityEditor;
using System;

public class FolderActionWindow : EditorWindow
{
    private string folderPath = "Assets";
    private string buttonLabel = "Do Action";
    private Action<string> onButtonClick; // Hành vi của nút bấm

    /// <summary>
    /// Mở window với tham số
    /// </summary>
    public static void Open(string title, string defaultFolder, string buttonText, Action<string> onClick)
    {
        var window = GetWindow<FolderActionWindow>(title);
        window.folderPath = defaultFolder;
        window.buttonLabel = buttonText;
        window.onButtonClick = onClick;
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("Folder Picker", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        folderPath = EditorGUILayout.TextField("Folder Path", folderPath);

        if (GUILayout.Button("...", GUILayout.MaxWidth(30)))
        {
            string selected = EditorUtility.OpenFolderPanel("Select Folder", folderPath, "");
            if (!string.IsNullOrEmpty(selected))
            {
                // Chuyển từ absolute path sang relative path trong Unity
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

        GUILayout.Space(10);

        if (GUILayout.Button(buttonLabel, GUILayout.Height(30)))
        {
            onButtonClick?.Invoke(folderPath);
        }
    }
}
