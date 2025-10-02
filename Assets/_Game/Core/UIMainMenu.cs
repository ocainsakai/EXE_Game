using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    private ISceneLoader sceneLoader => SceneLoader.Instance;

    [Header("Main Buttons")]
    [SerializeField] private Button start;        // VÀO TRẬN
    [SerializeField] private Button settings;     // CÀI ĐẶT
    [SerializeField] private Button deckButton;   // BỘ BÀI
    [SerializeField] private Button collection;   // BỘ SƯU TẬP
    [SerializeField] private Button shop;         // CỬA HÀNG

    private void OnEnable()
    {
        // VÀO TRẬN
        start.onClick.RemoveAllListeners();
        start.onClick.AddListener(() => sceneLoader.LoadSceneName("Map").Execute());

        // CÀI ĐẶT
        settings.onClick.RemoveAllListeners();
        settings.onClick.AddListener(()=> sceneLoader.LoadSceneName("Setting").Execute());

        // BỘ BÀI
        deckButton.onClick.RemoveAllListeners();
        deckButton.onClick.AddListener(OpenDeck);
        //deckButton.GetComponent<Image>().sprite = GameInstance.Singleton.GetDeckData().DeckCover;

        // BỘ SƯU TẬP
        collection.onClick.RemoveAllListeners();
        collection.onClick.AddListener(OpenCollection);

        // CỬA HÀNG
        shop.onClick.RemoveAllListeners();
        shop.onClick.AddListener(() => sceneLoader.LoadSceneName("Shop").Execute());
    }

    private void OpenDeck()
    {
        Debug.Log("Mở UI Bộ Bài");
        // TODO: hiện UI Deck Manager
    }

    private void OpenCollection()
    {
        Debug.Log("Mở UI Bộ Sưu Tập");
        // TODO: hiện UI Collection
    }

}
