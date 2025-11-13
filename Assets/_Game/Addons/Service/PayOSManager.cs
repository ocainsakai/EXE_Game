// FILE: PayOSManager.cs
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System; // Để dùng [Serializable] và Enum.Parse

// <<< SỬA 1: THÊM CÁC THƯ VIỆN FIREBASE >>>
using Firebase;
using Firebase.Extensions; // Cần cho Task.ContinueWithOnMainThread
using Firebase.Firestore;

/// <summary>
/// Quản lý việc TẠO đơn hàng PayOS VÀ LẮNG NGHE KẾT QUẢ.
/// </summary>
public class PayOSManager : MonoBehaviour
{
    public static PayOSManager Instance { get; private set; }

    [Header("Cấu hình Server")]
    [Tooltip("URL của Cloud Function 'create_payment'")]
    public string createPaymentUrl = "https://create-payment-tgyfsl6mrq-as.a.run.app";

    // Tham chiếu tới UI (giữ nguyên)
    private UIShop currentShopPanel;

    // Giá sản phẩm (giữ nguyên)
    private Dictionary<APProductKey, int> productPrices = new Dictionary<APProductKey, int>()
    {
        { APProductKey.coin500, 49000 },
        { APProductKey.coin1000, 99000 },
        { APProductKey.removeads, 49000 }
    };

    // ----- CÁC STRUCT JSON (giữ nguyên) -----
    [Serializable]
    private class PayOSCreateRequest
    {
        public int amount;
        public string userId;
        public string productId;
    }

    [Serializable]
    private class PayOSCreateResponse
    {
        public string paymentUrl;
        public string orderId;
    }
    
    // <<< SỬA 2: THÊM CLASS ĐỂ ĐỌC DỮ LIỆU TỪ FIRESTORE >>>
    [FirestoreData]
    private class OrderData
    {
        [FirestoreProperty]
        public string status { get; set; } // Sẽ là "PENDING" hoặc "PAID"

        [FirestoreProperty]
        public string productId { get; set; } // Sẽ là "coin500", "coin1000",...
    }

    // <<< SỬA 3: THÊM BIẾN FIREBASE VÀ LISTENER >>>
    private FirebaseFirestore db;
    private ListenerRegistration currentOrderListener; // Để theo dõi đơn hàng

    // ------------------------------------------

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // <<< SỬA 4: KHỞI TẠO FIREBASE >>>
        // Giả định FirebaseApp đã được khởi tạo bởi AuthManager hoặc 1 script khác
        db = FirebaseFirestore.DefaultInstance;
    }

    // Các hàm đăng ký UI (giữ nguyên)
    public void RegisterShopPanel(UIShop panel)
    {
        currentShopPanel = panel;
        Debug.Log("[PayOS] ShopPanel đã đăng ký.");
    }

    public void UnregisterShopPanel(UIShop panel)
    {
        if (currentShopPanel == panel)
        {
            currentShopPanel = null;
            Debug.Log("[PayOS] ShopPanel đã hủy đăng ký.");
        }
    }

    /// <summary>
    /// Bắt đầu quy trình mua hàng khi người dùng nhấn nút (giữ nguyên)
    /// </summary>
    public void BuyProduct(APProductKey key)
    {
        // 1. Kiểm tra User ID
        if (!AuthManager.IsLoggedIn || string.IsNullOrEmpty(AuthManager.UserId))
        {
            Debug.LogError("[PayOS] User chưa đăng nhập! Không thể thanh toán.");
            currentShopPanel?.OnPurchaseFailed("Lỗi xác thực, vui lòng thử lại.");
            return;
        }

        // 2. Kiểm tra giá
        if (!productPrices.ContainsKey(key))
        {
            Debug.LogError("[PayOS] Không tìm thấy giá cho: " + key);
            currentShopPanel?.OnPurchaseFailed("Lỗi giá sản phẩm.");
            return;
        }

        // 3. Bắt đầu Coroutine
        Debug.Log($"[PayOS] Bắt đầu tạo đơn hàng cho {key}...");
        StartCoroutine(CreateOrderAndOpenPayment(key, productPrices[key], AuthManager.UserId));
    }

    private IEnumerator CreateOrderAndOpenPayment(APProductKey productKey, int amount, string userId)
    {
        // Khóa UI ngay lập tức (giữ nguyên)
        currentShopPanel?.OnPurchaseStarted();

        // Chuẩn bị JSON (giữ nguyên)
        PayOSCreateRequest payload = new PayOSCreateRequest
        {
            amount = amount,
            userId = userId,
            productId = productKey.ToString()
        };
        string jsonPayload = JsonUtility.ToJson(payload);
        
        Debug.Log($"[PayOS] Gửi yêu cầu: {jsonPayload}");

        // Gửi request đến Cloud Function (giữ nguyên)
        using (UnityWebRequest www = new UnityWebRequest(createPaymentUrl, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonPayload);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

#if UNITY_2020_1_OR_NEWER
            if (www.result != UnityWebRequest.Result.Success)
#else
            if (www.isNetworkError || www.isHttpError)
#endif
            {
                Debug.LogError($"[PayOS] Lỗi tạo đơn hàng: {www.error} | {www.downloadHandler.text}");
                currentShopPanel?.OnPurchaseFailed("Không thể tạo đơn hàng. Vui lòng thử lại.");
                yield break;
            }

            // Parse kết quả trả về
            try
            {
                string jsonResponse = www.downloadHandler.text;
                PayOSCreateResponse response = JsonUtility.FromJson<PayOSCreateResponse>(jsonResponse);

                // <<< SỬA 5: SỬA LẠI LOG ERROR CHO ĐÚNG TÊN BIẾN >>>
                if (response == null || string.IsNullOrEmpty(response.paymentUrl))
                {
                    // Tên biến đúng là "paymentUrl", không phải "checkoutUrl"
                    Debug.LogError("[PayOS] Phản hồi không có 'paymentUrl': " + jsonResponse);
                    currentShopPanel?.OnPurchaseFailed("Lỗi máy chủ (1).");
                    yield break;
                }

                Debug.Log($"[PayOS] Tạo đơn hàng {response.orderId} thành công. Đang mở trình duyệt...");

                // Mở trình duyệt cho người dùng thanh toán
                Application.OpenURL(response.paymentUrl);

                // <<< SỬA 6: BẮT ĐẦU LẮNG NGHE THAY ĐỔI TỪ FIRESTORE >>>
                // Thay vì giữ UI khóa, chúng ta bắt đầu theo dõi đơn hàng
                StartListeningForOrder(response.orderId);

            }
            catch (Exception e)
            {
                Debug.LogError("[PayOS] Lỗi đọc JSON phản hồi: " + e.Message);
                currentShopPanel?.OnPurchaseFailed("Lỗi máy chủ (2).");
            }
        }
    }

    // <<< SỬA 7: THÊM HÀM MỚI ĐỂ LẮNG NGHE ĐƠN HÀNG >>>
    private void StartListeningForOrder(string orderId)
    {
        // Dừng listener cũ (nếu có)
        StopCurrentListener();

        Debug.Log($"[PayOS] Bắt đầu lắng nghe đơn hàng: {orderId}");
        DocumentReference orderRef = db.Collection("orders").Document(orderId);
        
        currentOrderListener = orderRef.Listen(snapshot =>
        {
            if (snapshot.Exists)
            {
                OrderData order = snapshot.ConvertTo<OrderData>();
                
                Debug.Log($"[PayOS] Trạng thái đơn hàng {orderId} thay đổi: {order.status}");

                if (order.status == "PAID")
                {
                    Debug.Log($"[PayOS] Đơn hàng {orderId} đã được thanh toán!");
                    
                    // Chuyển đổi productId (string) về APProductKey (enum)
                    APProductKey purchasedKey;
                    try
                    {
                        purchasedKey = (APProductKey)Enum.Parse(typeof(APProductKey), order.productId);
                    }
                    catch
                    {
                        Debug.LogError($"[PayOS] Không thể parse productId: {order.productId}");
                        currentShopPanel?.OnPurchaseFailed("Lỗi vật phẩm không xác định.");
                        StopCurrentListener();
                        return;
                    }

                    // Báo cho UI biết là thành công
                    currentShopPanel?.OnPurchaseSucceeded(purchasedKey.ToString());
                    
                    // Dừng lắng nghe
                    StopCurrentListener();
                }
                else if (order.status == "CANCELLED" || order.status == "EXPIRED")
                {
                    // Xử lý trường hợp server đánh dấu đơn hàng bị hủy (nếu có)
                    Debug.Log($"[PayOS] Đơn hàng {orderId} đã bị hủy hoặc hết hạn.");
                    currentShopPanel?.OnPurchaseFailed("Giao dịch bị hủy.");
                    StopCurrentListener();
                }
                // Nếu status vẫn là "PENDING", không làm gì cả, tiếp tục chờ...
            }
            else
            {
                // Điều này xảy ra nếu document bị xóa (lỗi)
                Debug.LogWarning($"[PayOS] Đơn hàng {orderId} không còn tồn tại.");
                StopCurrentListener();
            }
        });
    }

    // <<< SỬA 8: THÊM HÀM DỪNG LISTENER >>>
    private void StopCurrentListener()
    {
        if (currentOrderListener != null)
        {
            currentOrderListener.Stop();
            currentOrderListener = null;
            Debug.Log("[PayOS] Đã dừng lắng nghe đơn hàng.");
        }
    }

    // <<< SỬA 9: THÊM ONDESTROY ĐỂ DỌN DẸP >>>
    private void OnDestroy()
    {
        StopCurrentListener();
    }
}