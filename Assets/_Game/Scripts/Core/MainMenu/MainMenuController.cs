using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class MainMenuController : MonoBehaviour
{
   
    private IGameManager gameManager => GameManager.Instance;
    private ISceneLoader sceneLoader => SceneLoader.Instance;
    [SerializeField]
    private Button start;
    [SerializeField]
    private Button exit;

    private void OnEnable()
    {
        start.onClick.RemoveAllListeners();
        start.onClick.AddListener(() => sceneLoader.LoadSceneName("Map").Execute());
        exit.onClick.RemoveAllListeners();
        exit.onClick.AddListener(() => gameManager.QuitGame());
    }
}
