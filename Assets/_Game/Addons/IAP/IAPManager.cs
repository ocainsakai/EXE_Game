// Tên file: IAPManager.cs
// (Nội dung chính của video)

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

// [00:22:04] Các thư viện (namespace) cần thiết
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine.Purchasing;

// Bạn cũng sẽ cần tham chiếu đến 'ShopPanel' script của bạn
// using YourProject.UI; // Ví dụ

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

    private StoreController storeController; // [00:06:46]
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
        // [00:07:35] Bắt đầu khởi tạo IAP
        await InitIAP();
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
        // Hủy đăng ký tất cả các sự kiện
        if (storeController == null) return;

        storeController.OnStoreDisconnected -= OnStoreDisconnected;
        storeController.OnProductsFetched -= OnProductsFetched;
        storeController.OnProductsFetchFailed -= OnProductsFetchFailed;
        storeController.OnPurchasesFetched -= OnPurchasesFetched;
        storeController.OnPurchasesFetchFailed -= OnPurchasesFetchFailed;
        storeController.OnPurchasePending -= OnPurchasePending;
        storeController.OnPurchaseConfirmed -= OnPurchaseConfirmed;
        storeController.OnPurchaseFailed -= OnPurchaseFailed;
      //  storeController.OnPurchaseDeferred -= OnPurchaseDeferred;
        //storeController.OnCheckEntitlement -= OnCheckEntitlement;
    }

    // [00:07:35] Hàm khởi tạo chính (Async)
    private async Task InitIAP()
    {
        try
        {
            // [00:08:10] Đặt môi trường là "production"
            var option = new InitializationOptions().SetEnvironmentName("production");
            // [00:08:21] Khởi tạo Unity Services
            await UnityServices.InitializeAsync(option);

            // [00:08:32] Lấy Store Controller
            storeController = UnityIAPServices.StoreController();

            // [00:08:42] Đăng ký tất cả các sự kiện
            storeController.OnStoreDisconnected += OnStoreDisconnected;
            storeController.OnProductsFetched += OnProductsFetched;
            storeController.OnProductsFetchFailed += OnProductsFetchFailed;
            storeController.OnPurchasesFetched += OnPurchasesFetched;
            storeController.OnPurchasesFetchFailed += OnPurchasesFetchFailed;
            storeController.OnPurchasePending += OnPurchasePending;
            storeController.OnPurchaseConfirmed += OnPurchaseConfirmed;
            storeController.OnPurchaseFailed += OnPurchaseFailed;
        //    storeController.OnPurchaseDeferred += OnPurchaseDeferred;

            // [00:09:53] Kết nối đến Store
            await storeController.Connect();

            // [00:37:45] Đăng ký callback để kiểm tra khôi phục (cho non-consumable)
            RegisterEntitlementCallBack();
            
            // [00:11:20] Lấy thông tin các sản phẩm từ store
            var initialProductToFetch = BuildProductDefinitions();
            storeController.FetchProducts(initialProductToFetch);
        }
        catch (Exception e)
        {
            Debug.LogError($"Initialization failed with: {e}");
        }
    }

    private void RegisterEntitlementCallBack()
    {
        storeController.OnCheckEntitlement += OnCheckEntitlement;
    }
    private void OnCheckEntitlement(Entitlement result)
    {
        var product = result.Product;
        var status = result.Status;
        bool isEntitled = status == EntitlementStatus.FullyEntitled;
        // [00:39:19] Nếu người chơi "thực sự sở hữu"
        if (isEntitled)
        {
            // [00:39:31] Và nếu đó là gói "Remove Ads"
            if (product.definition.id == removeAdsProduct)
            {
                currentShopPanel.RemoveAdsReward();
            }
        }
    }
    // [00:10:25] Hàm định nghĩa các sản phẩm
    private List<ProductDefinition> BuildProductDefinitions()
    {
        return new List<ProductDefinition>
        {
            new ProductDefinition(coin500Product, ProductType.Consumable), // [00:10:58]
            new ProductDefinition(coin1000Product, ProductType.Consumable), // [00:11:10]
            new ProductDefinition(removeAdsProduct, ProductType.NonConsumable) // [00:11:10]
        };
    }

    // [00:14:26] Hàm được gọi từ các nút (Button) trong ShopPanel
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

        // [00:15:32] Bắt đầu quá trình mua hàng
        storeController.PurchaseProduct(productId);
    }

    #region --- Event Handlers (Các hàm xử lý sự kiện) ---

    // [00:11:35] Xử lý khi lấy sản phẩm thành công
    private void OnProductsFetched(List<Product> products)
    {
        Debug.Log("Products fetched successfully.");
        // [00:12:00] Lấy lịch sử mua hàng của người chơi
        storeController.FetchPurchases();

        if (currentShopPanel == null) return;   
        // [00:22:19] Cập nhật giá tiền lên các nút bấm
        foreach (var product in products)
        {
            // [00:22:27] Giả sử ShopPanel có hàm UpdateButtonPrice
            currentShopPanel.UpdateButtonPrice(product.definition.id, product.metadata.localizedPriceString);
        }
    }

    // [00:12:01] Xử lý khi lấy sản phẩm thất bại
    private void OnProductsFetchFailed(ProductFetchFailed reason)
    {
        Debug.LogError($"Failed to fetch products: {reason}"); // [00:12:17]
    }

    // [00:12:37] Xử lý khi lấy lịch sử mua hàng thành công
    private void OnPurchasesFetched(Orders purchases)
    {
        Debug.Log("Purchases fetched successfully.");
        isInitialized = true; // [00:12:56] Sẵn sàng để mua

        // [00:40:21] Đối với Android, chúng ta tự "Restore" bằng cách
        // kiểm tra lại tất cả các sản phẩm đã có.
        foreach (var product in storeController.GetProducts())
        {
            storeController.CheckEntitlement(product);
        }
    }

    // [00:13:05] Xử lý khi lấy lịch sử mua hàng thất bại
    private void OnPurchasesFetchFailed(PurchasesFetchFailureDescription reason)
    {
        Debug.LogError($"Failed to fetch purchases: {reason}"); // [00:13:24]
    }

    // [00:13:47] Xử lý khi mất kết nối
    private void OnStoreDisconnected(StoreConnectionFailureDescription  reason )
    {
        Debug.LogWarning($"Store disconnected. Reason: {reason}"); // [00:13:55]
    }

    // [00:18:07] Xử lý khi mua hàng THÀNH CÔNG
    private void OnPurchaseConfirmed(Order purchase)
    {
        // [00:35:15] Lấy số lượng từ biên lai (receipt)
        int quantity = GetPurchaseQuantity(purchase);

        if (currentShopPanel == null) return;   
        
        Debug.Log("PurchaseConfirmed");
        if (purchase?.Info?.PurchasedProductInfo != null && purchase.Info.PurchasedProductInfo.Count > 0)
        {
            string productionID = purchase.Info.PurchasedProductInfo[0].productId;
            switch (productionID)
            {
                case coin500Product:
                    currentShopPanel.AddRewardCoin(500 * quantity); // [00:37:28]
                    break;
                case coin1000Product:
                    currentShopPanel.AddRewardCoin(1000 * quantity); // [00:37:36]
                    break;
                case removeAdsProduct:
                    currentShopPanel.RemoveAdsReward(); // [00:19:41]
                    break;
            }
        }
        // [00:18:41] Trao thưởng
        
    }

    // [00:20:11] Xử lý khi mua hàng THẤT BẠI
    private void OnPurchaseFailed(FailedOrder product)
    {
        if (product?.Info?.PurchasedProductInfo == null || product.Info.PurchasedProductInfo.Count == 0)
        {
            Debug.Log("No product found.");
            return;
        }
        var production = product.Info.PurchasedProductInfo[0].productId;
        var reason = product.FailureReason;
        var message = product.Details;
        Debug.LogError($"Purchase failed for {production}. Reason: {reason}. Message: {message}"); // [00:20:31]
    }

    // [00:16:45] Xử lý khi giao dịch đang chờ (Pending)
    private void OnPurchasePending(PendingOrder product)
    {
        Debug.LogWarning($"Purchase pending for {product}. Confirming...");
        storeController.ConfirmPurchase(product); // [00:16:54]
    }
    
    #endregion

    #region --- Restore & Receipt Logic (Logic khôi phục & Biên lai) ---

    // [00:37:45] Đăng ký sự kiện kiểm tra quyền sở hữu
  

    // [00:38:27] Hàm được gọi khi kiểm tra 

    // [00:35:15] Hàm đọc biên lai (receipt) để lấy SỐ LƯỢNG (quantity)
    private int GetPurchaseQuantity(Order purchase)
    {
        int quantity = 1; // [00:35:35] Mặc định là 1
        if (string.IsNullOrEmpty(purchase.Info.Receipt)) return quantity;

        try
        {
            // [00:36:00] Đọc biên lai
            var apPayData = JsonUtility.FromJson<APPayData>(purchase.Info.Receipt);
            
            // [00:36:24] Chỉ phân tích nếu là biên lai thật từ Google Play
            if (apPayData.store == "GooglePlay")
            {
                // [00:36:38] Đọc "payload"
                var apPayload = JsonUtility.FromJson<APPayload>(apPayData.payload);
                // [00:36:38] Đọc data bên trong "payload"
                var apPayloadData = JsonUtility.FromJson<APPayloadData>(apPayload.json);
                
                // [00:37:12] Lấy số lượng
                quantity = apPayloadData.quantity;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to parse receipt JSON: {e.Message}");
        }
        
        return quantity;
    }

    // [00:40:54] Hàm Restore (Khôi phục)
    // Video nói: "[00:41:46] This function is primarily used for iOS devices"
    // Vì bạn không làm cho iOS, bạn KHÔNG CẦN hàm này.
    // Việc khôi phục cho Android đã được xử lý tự động trong OnPurchasesFetched.

    #endregion
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