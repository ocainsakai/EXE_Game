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
        // Kiểm tra null cho các button
        if (start != null)
        {
            start.onClick.RemoveAllListeners();

            // Kiểm tra sceneLoader trước khi sử dụng
            if (sceneLoader != null)
            {
                start.onClick.AddListener(() => sceneLoader.LoadSceneName("Map").Execute());
            }
            else
            {
                Debug.LogError("SceneLoader.Instance is null!");
            }
        }
        else
        {
            Debug.LogError("Start button is not assigned!");
        }

        if (exit != null)
        {
            exit.onClick.RemoveAllListeners();
            exit.onClick.AddListener(() => Application.Quit());
        }
        else
        {
            Debug.LogError("Exit button is not assigned!");
        }

        // Kiểm tra deckButton và các dependencies
        if (deckButton != null)
        {
            var deckImage = deckButton.GetComponent<Image>();
            if (deckImage != null)
            {
                // Kiểm tra GameInstance và DeckData
                if (GameInstance.Singleton != null)
                {
                    var deckData = GameInstance.Singleton.GetDeckData();
                    if (deckData != null && deckData.DeckCover != null)
                    {
                        deckImage.sprite = deckData.DeckCover;
                    }
                    else
                    {
                        Debug.LogError("DeckData or DeckCover is null!");
                    }
                }
                else
                {
                    Debug.LogError("GameInstance.Singleton is null!");
                }
            }
            else
            {
                Debug.LogError("DeckButton doesn't have Image component!");
            }
        }
        else
        {
            Debug.LogError("Deck button is not assigned!");
        }
    }

    // Alternative: Sử dụng Start() thay vì OnEnable() nếu cần đảm bảo các Singleton đã được khởi tạo
    private void Start()
    {
        // Di chuyển logic từ OnEnable() xuống đây nếu cần
    }
}