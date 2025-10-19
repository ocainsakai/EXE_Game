using _Game.Core.Gameplay;
using UnityEditor;
using UnityEngine;

public class GameInstanceHelper : MonoBehaviour
{
#if UNITY_EDITOR
    public void LoadScene(SceneAsset sceneAsset)
    {
        SceneLoader.Instance.LoadScene(sceneAsset.name);
    }
#endif
}
