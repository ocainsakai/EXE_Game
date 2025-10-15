using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    private ISceneLoader sceneLoader => SceneLoader.Instance;

    [Header("Top Panel")]
    [SerializeField] private Text coinText;
    [SerializeField] private Text goldText;

    [Header("Left Menu Buttons")]
    [SerializeField] private Button goldButton;
    [SerializeField] private Button cardButton;
    [SerializeField] private Button timeButton;

    [Header("Back Button")]
    [SerializeField] private Button backButton;

    private void OnEnable()
    {
        // Gán sự kiện cho các nút shop
        goldButton.onClick.RemoveAllListeners();
        goldButton.onClick.AddListener(OpenGoldShop);

        cardButton.onClick.RemoveAllListeners();
        cardButton.onClick.AddListener(OpenCardShop);

        timeButton.onClick.RemoveAllListeners();
        timeButton.onClick.AddListener(OpenTimeShop);

        // Gán sự kiện cho nút Back
        backButton.onClick.RemoveAllListeners();
        backButton.onClick.AddListener(() =>
        {
            sceneLoader.LoadSceneName("MainMenu").Execute();
        });

        // Cập nhật coin/gold khi mở Shop
        UpdateCurrencyUI();
    }

    private void UpdateCurrencyUI()
    {
        var playerData = GameInstance.Singleton.PlayerData;
        //coinText.text = playerData.Coin.ToString();
        //goldText.text = playerData.Gold.ToString();
    }

    private void OpenGoldShop()
    {
        Debug.Log("Mở Shop Gold");
        // TODO: load UI content cho gold
    }

    private void OpenCardShop()
    {
        Debug.Log("Mở Shop Card");
        // TODO: load UI content cho card
    }

    private void OpenTimeShop()
    {
        Debug.Log("Mở Shop Time/Item");
        // TODO: load UI content cho time
    }
}
