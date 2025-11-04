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
    [SerializeField] private GameObject goldPanel;
    [SerializeField] private GameObject cardPanel;
    [SerializeField] private GameObject timePanel;
    private void OnEnable()
    {
        // Gán sự kiện cho các nút shop
        goldButton.onClick.RemoveAllListeners();
        goldButton.onClick.AddListener(OpenGoldShop);

        cardButton.onClick.RemoveAllListeners();
        cardButton.onClick.AddListener(OpenCardShop);

        timeButton.onClick.RemoveAllListeners();
        timeButton.onClick.AddListener(OpenTimeShop);
        
        UpdateCurrencyUI();
    }

    private void CloseAllUI()
    {
        goldPanel.SetActive(false);
        cardPanel.SetActive(false);
        timePanel.SetActive(false);
    }
    private void UpdateCurrencyUI()
    { 
        Debug.Log(PlayerSave.GetPlayerCoin());
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
        coinText.text = PlayerSave.GetPlayerCoin().ToString(); // [00:02:07]
    }

    // --- CÁC HÀM ĐƯỢC GỌI TỪ NÚT BẤM (UI) ---
    // [00:25:15] Gắn các hàm này vào OnClick của các Button tương ứng

    public void OnBuy500CoinsClicked()
    {
        IAPManager.Instance.BuyProduct(APProductKey.coin500);
    }

    public void OnBuy1000CoinsClicked()
    {
        IAPManager.Instance.BuyProduct(APProductKey.coin1000);
    }

    public void OnBuyRemoveAdsClicked()
    {
        IAPManager.Instance.BuyProduct(APProductKey.removeads);
    }

    // --- CÁC HÀM ĐƯỢC GỌI TỪ IAPMANAGER ---

    // [00:23:10] Hàm này được IAPManager gọi để cập nhật giá
    public void UpdateButtonPrice(string productID, string price)
    {
        // [00:24:21] Video dùng tên ID khớp với IAPModel
        // (Lưu ý: Tên hằng số "coin500product" ở IAPManager khác với 
        // enum "coin500" ở IAPModel. Cần đảm bảo bạn dùng đúng ID)
        
        // Chúng ta sẽ dùng ID từ IAPManager
        string coin500ID = "coin500product";   // Lấy từ IAPManager.cs
        string coin1000ID = "coin1000product"; // Lấy từ IAPManager.cs
        string removeAdsID = "removeadsproduct"; // Lấy từ IAPManager.cs

        if (productID == coin500ID)
        {
            //    coins500PriceText.text = price; // [00:24:29]
        }
        else if (productID == coin1000ID)
        {
          //  coins1000PriceText.text = price; // [00:24:37]
        }
        else if (productID == removeAdsID)
        {
            //removeAdsPriceText.text = price; // [00:24:45]
        }
    }

    // [00:19:04] Hàm này được IAPManager gọi khi mua coin thành công
    public void AddRewardCoin(int amount)
    {
        PlayerSave.AddCoin(amount);    
        UpdateCoinUI();
    }

    // [00:19:41] Hàm này được IAPManager gọi khi mua "Remove Ads" thành công
    public void RemoveAdsReward()
    {
        // [00:03:28] Lưu trạng thái đã gỡ quảng cáo
     //   PlayerPrefs.SetInt(REMOVE_ADS_KEY, 1);
       // PlayerPrefs.Save();

        // Tắt quảng cáo trong game (ví dụ)
        // AdManager.Instance.DisableAds();

        // Cập nhật UI
        UpdateRemoveAdsUI();
        
        Debug.Log("Đã gỡ quảng cáo!");
    }
    
    // [00:03:28] Hàm riêng để cập nhật UI của nút "Remove Ads"
    private void UpdateRemoveAdsUI()
    {
        timeButton.interactable = false; // Tắt nút
        //timePanel.text = "ACTIVATED"; // Đổi chữ [00:02:21]
        // Bạn cũng có thể đổi màu nút ở đây [00:02:21]
        // removeAdsButton.GetComponent<Image>().color = Color.green;
    }
}
