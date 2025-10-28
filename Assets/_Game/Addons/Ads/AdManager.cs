using System;
using System.IO;
using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.UI;

public class AdManager : MonoBehaviour
{
    // Singleton instance

    // Ad Unit IDs (use test IDs for development)
#if UNITY_ANDROID
    private string bannerAdUnitId = "ca-app-pub-3940256099942544/6300978111";
    private string interestitialAdUnitId = "ca-app-pub-3940256099942544/1033173712";
    private string rewardedAdUnitId = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IPHONE
    private string bannerAdUnitId = "ca-app-pub-3940256099942544/2934735716";
    private string interestitialAdUnitId = "ca-app-pub-3940256099942544/4411468910";
    private string rewardedAdUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
    private string bannerAdUnitId = "unused";
    private string interestitialAdUnitId = "unused";
    private string rewardedAdUnitId = "unused";
#endif

    // Ad objects
    private BannerView bannerView;
    private InterstitialAd interstitialAd;
    private RewardedAd rewardedAd;

    // --- Cải tiến: Thêm các hàm Callbacks ---
    private Action _onInterstitialClosed;
    private Action _onUserEarnedReward;
    
    // --- Cải tiến: Thêm thuộc tính kiểm tra trạng thái ---
    public bool IsInterstitialAdReady => interstitialAd != null && interstitialAd.CanShowAd();
    public bool IsRewardedAdReady => rewardedAd != null && rewardedAd.CanShowAd();
    

    public static AdManager Instance
    {
        get
        {
            if (_instance) return _instance;

            var go = new GameObject(nameof(AdManager));
            _instance = go.AddComponent<AdManager>();
            DontDestroyOnLoad(go);
            return _instance;
        }
    }
    private static AdManager _instance;
    void Start()
    {
        // Initialize the google mobile ads SDK
        MobileAds.Initialize(initStatus =>
        {
            Debug.Log("Admob SDK Initialized");
            //Load ads after initialization
            LoadBannerAd();
            LoadInterstitialAd();
            LoadRewardedAd();
        });
    }

    private void OnDestroy()
    {
        // --- Cải tiến: Dọn dẹp để tránh rò rỉ bộ nhớ ---
        if (bannerView != null) bannerView.Destroy();
        if (interstitialAd != null) interstitialAd.Destroy();
        if (rewardedAd != null) rewardedAd.Destroy();
    }

    #region Banner Ad

    private void LoadBannerAd()
    {
        // Create a banner view at the bottom of the screen
        bannerView = new BannerView(bannerAdUnitId, AdSize.Banner, AdPosition.Bottom);

        // Create an empty ad request
        AdRequest request = new AdRequest();

        // Register event handlers for the banner ad
        bannerView.OnBannerAdLoaded += () => Debug.Log("Banner ad loaded.");
        bannerView.OnBannerAdLoadFailed += (LoadAdError error) => Debug.Log("Banner ad failed to load: " + error.GetMessage());

        // Load the banner ad
        //bannerView.LoadAd(request);
    }

    #endregion

    #region Interstitial Ad

    private void LoadInterstitialAd()
    {
        // Clean up any existing interstitial ad
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }

        Debug.Log("Loading interstitial ad...");
        
        // Load a new interstitial ad
        InterstitialAd.Load(interestitialAdUnitId, new AdRequest(), (InterstitialAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Debug.LogError("Interstitial ad failed to load: " + error?.GetMessage());
                // --- Cải tiến: Tự động thử lại sau 5s ---
                RetryLoadInterstitialAd();
                return;
            };

            interstitialAd = ad;
            Debug.Log("Interstitial ad loaded");

            // Register ad events
            interstitialAd.OnAdFullScreenContentClosed += HandleInterstitialClosed;
            interstitialAd.OnAdFullScreenContentFailed += HandleInterstitialFailed;
        });
    }
    
    // --- Cải tiến: Hàm này nhận một hàm callback (hàm gọi lại) ---
    public void ShowInterstialAd(Action onAdClosed = null)
    {
        _onInterstitialClosed = onAdClosed; // Lưu hàm callback lại
        
        if (IsInterstitialAdReady)
        {
            interstitialAd.Show();
        }
        else
        {
            Debug.Log("Interstitial ad not ready");
            // Nếu không sẵn sàng, vẫn gọi callback (nếu có) để logic game tiếp tục
            _onInterstitialClosed?.Invoke();
            _onInterstitialClosed = null;
        }
    }

    private void RetryLoadInterstitialAd()
    {
        Debug.Log("Retrying to load interstitial ad in 5 seconds...");
        Invoke(nameof(LoadInterstitialAd), 5.0f); // Thử lại sau 5s
    }

    // --- Cải tiến: Tách hàm xử lý sự kiện ra riêng ---
    private void HandleInterstitialClosed()
    {
        Debug.Log("Interstitial ad closed");
        _onInterstitialClosed?.Invoke(); // Gọi hàm callback đã lưu
        _onInterstitialClosed = null;    // Xóa callback để tránh gọi nhầm
        LoadInterstitialAd(); // Tải trước (preload) quảng cáo tiếp theo
    }
    
    private void HandleInterstitialFailed(AdError error)
    {
        Debug.Log("Interstitial ad failed to show: " + error.GetMessage());
        LoadInterstitialAd(); // Tải lại nếu quảng cáo bị lỗi khi hiển thị
    }

    #endregion

    #region Rewarded Ad

    private void LoadRewardedAd()
    {
        // Clean up any existing rewarded ad
        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
            rewardedAd = null;
        }
        
        Debug.Log("Loading rewarded ad...");

        // Load a new rewarded ad
        RewardedAd.Load(rewardedAdUnitId, new AdRequest(), (RewardedAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Debug.LogError("Rewarded ad failed to load:" + error?.GetMessage());
                // --- Cải tiến: Tự động thử lại sau 5s ---
                RetryLoadRewardedAd();
                return;
            }

            rewardedAd = ad;
            Debug.Log("Rewarded ad loaded.");

            // Register ad events
            rewardedAd.OnAdFullScreenContentClosed += HandleRewardedAdClosed;
            rewardedAd.OnAdFullScreenContentFailed += HandleRewardedAdFailed;
        });
    }

    // --- Cải tiến: Hàm này nhận một hàm callback cho việc nhận thưởng ---
    public void ShowRewardedAd(Action onUserEarnedReward)
    {
        if (IsRewardedAdReady)
        {
            _onUserEarnedReward = onUserEarnedReward; // Lưu hàm callback lại
            
            rewardedAd.Show((Reward reward) =>
            {
                // Sự kiện này chỉ được gọi khi người dùng XEM HẾT
                Debug.Log($"User earned reward: {reward.Amount} {reward.Type}");
                _onUserEarnedReward?.Invoke(); // Gọi hàm callback đã lưu
                _onUserEarnedReward = null;    // Xóa callback
            });
        }
        else
        {
            Debug.Log("Rewarded ad not ready");
            // Không làm gì cả, người dùng không thể xem quảng cáo
        }
    }
    
    private void RetryLoadRewardedAd()
    {
        Debug.Log("Retrying to load rewarded ad in 5 seconds...");
        Invoke(nameof(LoadRewardedAd), 5.0f); // Thử lại sau 5s
    }
    
    // --- Cải tiến: Tách hàm xử lý sự kiện ra riêng ---
    private void HandleRewardedAdClosed()
    {
        // Sự kiện này được gọi khi quảng cáo đóng, BẤT KỂ là có nhận thưởng hay không
        Debug.Log("Rewarded ad closed.");
        
        // Nếu người dùng đóng quảng cáo giữa chừng (chưa nhận thưởng)
        // callback _onUserEarnedReward sẽ vẫn còn
        if (_onUserEarnedReward != null)
        {
            Debug.Log("User closed ad early, no reward given.");
            _onUserEarnedReward = null; // Hủy callback vì không có thưởng
        }
        
        LoadRewardedAd(); // Tải trước (preload) quảng cáo tiếp theo
    }
    
    private void HandleRewardedAdFailed(AdError error)
    {
        Debug.LogError("Rewarded ad failed to show:" + error.GetMessage());
        LoadRewardedAd(); // Tải lại nếu quảng cáo bị lỗi khi hiển thị
    }

    #endregion
}