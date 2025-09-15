using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class MainMenuController : MonoBehaviour
{
    private IGameManager gameManager;
    private ISceneLoader sceneLoader;
    [SerializeField]
    private Button start;
    [SerializeField]
    private Button exit;
    [Inject]
    public void Construct(IGameManager gameManager, ISceneLoader sceneLoader)
    {
        this.gameManager = gameManager;
        this.sceneLoader = sceneLoader;
    }

    private void OnEnable()
    {
        start.onClick.RemoveAllListeners();
        start.onClick.AddListener(() => sceneLoader.LoadSceneName("Map").Execute());
        exit.onClick.RemoveAllListeners();
        exit.onClick.AddListener(() => gameManager.QuitGame());
    }
}
