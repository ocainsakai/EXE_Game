using System;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    public static AdsManager Instance;
    
    // --- CẤU HÌNH ---
    [SerializeField] string _androidGameId = "5485920"; // Thay đúng ID 7 số của bạn
    
    // QUAN TRỌNG: Chỉ tắt Test Mode khi Build bản cuối cùng để up lên mạng
    // Khi đang code và test trên máy mình thì nên để true
    [SerializeField] bool _testMode = true; 

    string _interstitialAdId = "Interstitial_Android"; 
    string _rewardedAdId = "Rewarded_Android";
    
    private Action _onRewardComplete;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Giữ AdsManager không bị mất khi chuyển cảnh
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Tự động bật chế độ Test nếu đang chạy trong Unity Editor hoặc bản Development Build
        if (Debug.isDebugBuild) 
        {
            _testMode = true;
        }

        InitializeAds();
    }

    public void InitializeAds()
    {
        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(_androidGameId, _testMode, this);
        }
    }

    // 1. Gọi Quảng cáo Thưởng (Hồi sinh, Nhận vàng...)
    public void ShowRewardedAd(Action onSuccess)
    {
        _onRewardComplete = onSuccess;
        Debug.Log("Đang tải quảng cáo thưởng...");
        Advertisement.Load(_rewardedAdId, this);
    }

    // 2. Gọi Quảng cáo xen kẽ (Chuyển màn, Thua game...)
    public void ShowAd()
    {
        Debug.Log("Đang tải quảng cáo xen kẽ...");
        Advertisement.Load(_interstitialAdId, this);
    }

    // --- INTERFACE ---

    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        // Tải xong hiện luôn
        Advertisement.Show(adUnitId, this);
    }

    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState) 
    {
        if (showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            // SỬA LỖI: Kiểm tra đúng ID của loại quảng cáo Thưởng
            if (adUnitId.Equals(_rewardedAdId))
            {
                Debug.Log("Đã xem xong Reward Video. Trao thưởng!");
                if (_onRewardComplete != null)
                {
                    _onRewardComplete.Invoke();
                    _onRewardComplete = null;
                }
            }
            // Kiểm tra nếu là quảng cáo xen kẽ (thường không có thưởng)
            else if (adUnitId.Equals(_interstitialAdId))
            {
                Debug.Log("Đã xem xong Interstitial Ad.");
            }
        }
    }

    // Các hàm báo lỗi
    public void OnInitializationComplete() { Debug.Log("Ads Init Success"); }
    public void OnInitializationFailed(UnityAdsInitializationError error, string message) { Debug.Log($"Init Failed: {message}"); }
    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message) { Debug.Log($"Load Failed: {message}"); }
    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message) { Debug.Log($"Show Failed: {message}"); }
    public void OnUnityAdsShowStart(string adUnitId) { }
    public void OnUnityAdsShowClick(string adUnitId) { }
}