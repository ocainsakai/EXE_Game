using System;
using UnityEngine;
using UnityEngine.Advertisements; // Thư viện quan trọng

public class AdsManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    public static AdsManager Instance; 
    [SerializeField] string _androidGameId = "1234567"; // Thay ID của bạn vào đây
    [SerializeField] bool _testMode = true; // ĐỂ TRUE KHI TEST, SỬA THÀNH FALSE KHI XUẤT APK
    string _adUnitId = "Interstitial_Android"; // Hoặc "Rewarded_Android"
    private string _rewardedAdUnitId = "Rewarded_Android";
    private Action _onRewardComplete;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
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
    public void ShowRewardedAd(Action onSuccess)
    {
        _onRewardComplete = onSuccess;

        Debug.Log("Đang tải quảng cáo thưởng...");
        Advertisement.Load(_rewardedAdUnitId, this);
    }
    // Hàm này gọi khi bạn muốn hiện quảng cáo (gắn vào nút bấm hoặc khi thua game)
    public void ShowAd()
    {
        Debug.Log("Đang tải quảng cáo...");
        Advertisement.Load(_adUnitId, this);
    }

    // --- Các hàm Interface bắt buộc (Copy y nguyên là chạy) ---

    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads khởi tạo thành công.");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Lỗi khởi tạo: {error.ToString()} - {message}");
    }

    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        // Tải xong thì hiện luôn
        Advertisement.Show(adUnitId, this);
    }

    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Lỗi tải quảng cáo: {message}");
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message) { }

    public void OnUnityAdsShowStart(string adUnitId) { }

    public void OnUnityAdsShowClick(string adUnitId) { }

    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState) 
    {
        if (adUnitId.Equals(_adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            Debug.Log("Người dùng đã xem hết quảng cáo. Thưởng quà thôi!");
            if (_onRewardComplete != null)
            {
                _onRewardComplete.Invoke(); // Kích hoạt đoạn code trong UILoseScreen
                _onRewardComplete = null;   // Reset để tránh lỗi lần sau
            }
        }
    }
}