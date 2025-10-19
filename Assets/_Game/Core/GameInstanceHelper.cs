using _Game.Core.Gameplay;
using UnityEditor;
using UnityEngine;

public class GameInstanceHelper : MonoBehaviour
{
    public void LoadScene(SceneAsset sceneAsset)
    {
        SceneLoader.Instance.LoadScene(sceneAsset.name);
    }
}
