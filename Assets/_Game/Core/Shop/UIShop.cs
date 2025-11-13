using _Game.Core;
using _Game.Core.Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIShop : MonoBehaviour
{
    [Header("Top Panel")]
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI goldText;

    [Header("Left Menu Buttons")]
    [SerializeField] private Button goldButton;
    [SerializeField] private Button cardButton;
    [SerializeField] private Button timeButton;

    [Header("UIs")]
    [SerializeField] private UIPaymentSuccess uiPaymentSuccess;
    [SerializeField] private GameObject goldPanel;
    [SerializeField] private GameObject cardPanel;
    [SerializeField] private GameObject timePanel;

    [Header("Messages")]
    [SerializeField] private TextMeshProUGUI statusText; // Thêm UI text để hiển thị trạng thái (nếu có)

    private void OnEnable()
    {
        // Gán sự kiện cho các nút shop
        goldButton.onClick.RemoveAllListeners();
        goldButton.onClick.AddListener(OnBuy500CoinsClicked);

        cardButton.onClick.RemoveAllListeners();
        cardButton.onClick.AddListener(OnBuy1000CoinsClicked);

        timeButton.onClick.RemoveAllListeners();
        timeButton.onClick.AddListener(OnBuyRemoveAdsClicked);

        // Đăng ký shop panel cho PayOSManager
        if (PayOSManager.Instance != null)
            PayOSManager.Instance.RegisterShopPanel(this);

        UpdateCurrencyUI();
    }

    private void OnDisable()
    {
        if (PayOSManager.Instance != null)
            PayOSManager.Instance.UnregisterShopPanel(this);
    }

    private void CloseAllUI()
    {
        goldPanel.SetActive(false);
        cardPanel.SetActive(false);
        timePanel.SetActive(false);
    }

    private void UpdateCurrencyUI()
    {
        coinText.text = PlayerSave.GetPlayerCoin().ToString();
    }

    private void OpenGoldShop()
    {
        CloseAllUI();
        goldPanel.SetActive(true);
    }

    private void OpenCardShop()
    {
        CloseAllUI();
        cardPanel.SetActive(true);
    }

    private void OpenTimeShop()
    {
        CloseAllUI();
        timePanel.SetActive(true);
    }

    private void UpdateCoinUI()
    {
        coinText.text = PlayerSave.GetPlayerCoin().ToString();
    }

    // --- Các hàm gọi mua hàng ---
    public void OnBuy500CoinsClicked()
    {
        Debug.Log("[UIShop] Mua 500 coins");
        PayOSManager.Instance.BuyProduct(APProductKey.coin500);
    }

    public void OnBuy1000CoinsClicked()
    {
        Debug.Log("[UIShop] Mua 1000 coins");
        PayOSManager.Instance.BuyProduct(APProductKey.coin1000);
    }

    public void OnBuyRemoveAdsClicked()
    {
        Debug.Log("[UIShop] Mua gói Remove Ads");
        PayOSManager.Instance.BuyProduct(APProductKey.removeads);
    }

    // --- CALLBACKS từ PayOSManager ---
    public void OnPurchaseStarted()
    {
        if (statusText != null)
            statusText.text = "🔄 Đang mở trang thanh toán...";
        SetButtonsInteractable(false);
    }

    public void OnPurchaseSucceeded(string message)
    {
        Debug.Log("[UIShop] Thanh toán thành công: " + message);

        if (statusText != null)
            statusText.text = "✅ " + message;

        uiPaymentSuccess.gameObject.SetActive(true);
        uiPaymentSuccess.Show("Giao dịch thành công.", message);
        UpdateCoinUI();
        SetButtonsInteractable(true);
    }

    public void OnPurchaseFailed(string message)
    {
        Debug.LogWarning("[UIShop] Thanh toán thất bại: " + message);

        if (statusText != null)
            statusText.text = "❌ " + message;

        SetButtonsInteractable(true);
    }

    // --- Hỗ trợ cập nhật UI Remove Ads ---
    public void RemoveAdsReward()
    {
        UpdateRemoveAdsUI();
        if (statusText != null)
            statusText.text = "🚫 Quảng cáo đã bị gỡ!";
    }

    private void UpdateRemoveAdsUI()
    {
        timeButton.interactable = false; // Tắt nút
        // Bạn có thể đổi màu hoặc chữ ở đây
        // timeButton.GetComponentInChildren<TextMeshProUGUI>().text = "Đã kích hoạt";
    }

    // --- Helper ---
    private void SetButtonsInteractable(bool active)
    {
      
        goldButton.interactable = active;
        cardButton.interactable = active;
        timeButton.interactable = active;
    }
}
