using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    private ISceneLoader sceneLoader => SceneLoader.Instance;
    [SerializeField]
    private Button start;
    [SerializeField]
    private Button exit;
    [SerializeField] 
    private Button settings;
    [SerializeField]
    private Button deckButton;
    private void OnEnable()
    {
        start.onClick.RemoveAllListeners();
        start.onClick.AddListener(() => sceneLoader.LoadSceneName("Map").Execute());
        exit.onClick.RemoveAllListeners();
        exit.onClick.AddListener(() => Application.Quit());
        deckButton.GetComponent<Image>().sprite = GameInstance.Singleton.GetDeckData().CardBack;
    }
}
