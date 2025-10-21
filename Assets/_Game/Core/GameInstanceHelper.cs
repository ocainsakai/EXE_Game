using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using _Game.Core.Gameplay;

public class GameInstanceHelper : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] private SceneAsset sceneAsset; // Chỉ hiện trong Editor
#endif
    [SerializeField] private string sceneName; // Dùng khi build

#if UNITY_EDITOR
    private void OnValidate()
    {
        // Đồng bộ sceneName mỗi khi đổi sceneAsset trong Editor
        if (sceneAsset != null)
        {
            sceneName = sceneAsset.name;
        }
    }
#endif

    public void LoadScene()
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogWarning("Scene name is empty! Cannot load scene.");
            return;
        }

        SceneLoader.Instance.LoadScene(sceneName);
    }
}

