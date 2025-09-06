using UnityEngine;
using UnityEditor;

public static class PopupConfigTool
{
    [MenuItem("Tools/UI/Open PopupConfig Generator")]
    public static void OpenConfigGenerator()
    {
        FolderActionWindow.Open(
            "Popup Config Generator",
            "Assets/Resources/PopupConfigs",
            "Generate Config",
            (folderPath) =>
            {
                var config = ScriptableObject.CreateInstance<PopupConfig>();
                config.size = new Vector2(400, 200);
                config.backgroundColor = Color.gray;

                string assetPath = $"{folderPath}/NewPopupConfig.asset";
                AssetDatabase.CreateAsset(config, assetPath);
                AssetDatabase.SaveAssets();

                EditorUtility.DisplayDialog("Success", $"Config saved to {assetPath}", "OK");
            });
    }
}
