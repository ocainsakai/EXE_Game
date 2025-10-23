using UnityEngine;
using _Game.Core.Gameplay;

public class GameInstanceHelper : MonoBehaviour
{
    [SerializeField] private string mapSceneName ="Map" ; 
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    public void LoanMapScene()
    {
        if (string.IsNullOrEmpty(mapSceneName))
        {
            Debug.LogWarning("Scene name is empty! Cannot load scene.");
            return;
        }

        SceneLoader.Instance.LoadScene(mapSceneName);
    }
    public void LoanMainMenuScene()
    {
        if (string.IsNullOrEmpty(mainMenuSceneName))
        {
            Debug.LogWarning("Scene name is empty! Cannot load scene.");
            return;
        }

        SceneLoader.Instance.LoadScene(mainMenuSceneName);
    }
}

