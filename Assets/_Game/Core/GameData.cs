using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Game Data", menuName = "GameDatas/GameData")]
public class GameData : ScriptableObject
{
    [Header("Folder chứa các ScriptableObject")]
    public DefaultAsset dataFolder; // Kéo folder vào đây

    [SerializeField] private ScriptableObject[] _gameData;

    public T GetData<T>() where T : ScriptableObject
    {
        return _gameData.OfType<T>().FirstOrDefault();
    }

    public T[] GetAllData<T>() where T : ScriptableObject
    {
        return _gameData.OfType<T>().ToArray();
    }

#if UNITY_EDITOR
    /// <summary>
    /// Reload toàn bộ ScriptableObject từ folder.
    /// </summary>
    public void ReloadDataFromFolder()
    {
        if (dataFolder == null) return;

        string folderPath = UnityEditor.AssetDatabase.GetAssetPath(dataFolder);
        string[] guids = UnityEditor.AssetDatabase.FindAssets("t:ScriptableObject", new[] { folderPath });

        _gameData = guids
            .Select(g => UnityEditor.AssetDatabase.LoadAssetAtPath<ScriptableObject>(
                UnityEditor.AssetDatabase.GUIDToAssetPath(g)))
            .ToArray();

        UnityEditor.EditorUtility.SetDirty(this); // đánh dấu asset thay đổi
    }
#endif
}
