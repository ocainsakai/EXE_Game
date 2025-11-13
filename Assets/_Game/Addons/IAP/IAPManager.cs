// Tên file: IAPManager.cs
// (Nội dung chính của video)

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;



public class IAPManager : MonoBehaviour
{
    // [00:06:08] Singleton
    public static IAPManager Instance { get; private set; }

    // [00:06:58] Tham chiếu đến script ShopPanel để trao thưởng
    private UIShop currentShopPanel;

    // [00:06:15] ID sản phẩm (PHẢI KHỚP VỚI GOOGLE PLAY)
    private const string coin500Product = "coin500product";   // [00:06:22]
    private const string coin1000Product = "coin1000product"; // [00:06:22]
    private const string removeAdsProduct = "removeadsproduct"; // [00:06:22]

    private bool isInitialized = false; // [00:06:36]

    private void Awake()
    {
        // [00:07:05] Cài đặt Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // [00:07:13]
        }
    }

    private async void Start()
    {
;
    }
    public void RegisterShopPanel(UIShop panel)
    {
        currentShopPanel = panel;
        Debug.Log("ShopPanel đã đăng ký với IAPManager.");
    
        // (Tùy chọn) Cập nhật giá ngay khi shop vừa mở
        // Bạn có thể tạo một hàm mới để làm việc này
        // UpdatePricesOnShop(); 
    }
    public void UnregisterShopPanel(UIShop panel)
    {
        // Chỉ unregister nếu đúng là nó
        if (currentShopPanel == panel)
        {
            currentShopPanel = null;
            Debug.Log("ShopPanel đã hủy đăng ký.");
        }
    }
    private void OnDestroy()
    {
      
    }
    public void BuyProduct(APProductKey key)
    {
        if (!isInitialized) // [00:15:32]
        {
            Debug.LogError("IAP not initialized.");
            return;
        }
        
        Debug.Log("BuyProduct");

        string productId = "";
        switch (key)
        {
            case APProductKey.coin500:
                productId = coin500Product;
                break;
            case APProductKey.coin1000:
                productId = coin1000Product;
                break;
            case APProductKey.removeads:
                productId = removeAdsProduct;
                break;
        }

    }

}


// Tên file: IAPModel.cs
// (Class này được mô tả từ [00:14:42] và [00:33:05] trong video)
// [00:14:56] Enum để gọi từ các nút bấm
public enum APProductKey
{
    coin500,    // [00:15:11]
    coin1000,
    removeads
}

// --- Các class để phân tích biên lai (Receipt) ---
// [00:33:14]
[Serializable]
public class APPayData
{
    public string payload;
    public string store;
    public string transactionID;
}

// [00:33:44]
[Serializable]
public class APPayload
{
    public string json;
    public string signature;
}

// [00:33:52]
[Serializable]
public class APPayloadData
{
    public string orderId;
    public string packageName;
    public string productId;
    public long purchaseTime;
    public int purchaseState;
    public string purchaseToken;
    public int quantity; // [00:34:11] Đây là trường quan trọng
    public bool acknowledged;
}