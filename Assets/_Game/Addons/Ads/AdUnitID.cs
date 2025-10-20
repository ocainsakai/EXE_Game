using UnityEngine;

[CreateAssetMenu(fileName = "AdUnitID", menuName = "Scriptable Objects/AdUnitID")]
public class AdUnitID : ScriptableObject
{
    [Header("App IDs")]
    public string androidAppId;
    public string iosAppId;

    [Header("Android Ad Unit IDs")]
    public string androidAppOpen = "ca-app-pub-3940256099942544/9257395921";
    public string androidBanner = "ca-app-pub-3940256099942544/6300978111";
    public string androidAdaptiveBanner = "ca-app-pub-3940256099942544/9214589741";
    public string androidInterstitial = "ca-app-pub-3940256099942544/1033173712";
    public string androidRewarded = "ca-app-pub-3940256099942544/5224354917";
    public string androidRewardedInterstitial = "ca-app-pub-3940256099942544/5354046379";
    public string androidNative = "ca-app-pub-3940256099942544/2247696110";

    [Header("iOS Ad Unit IDs")]
    public string iosAppOpen;
    public string iosBanner;
    public string iosAdaptiveBanner;
    public string iosInterstitial;
    public string iosRewarded;
    public string iosRewardedInterstitial;
    public string iosNative;

    public string GetAdUnitId(AdType type)
    {
        bool isAndroid = Application.platform == RuntimePlatform.Android;
        switch (type)
        {
            case AdType.AppOpen: return isAndroid ? androidAppOpen : iosAppOpen;
            case AdType.Banner: return isAndroid ? androidBanner : iosBanner;
            case AdType.AdaptiveBanner: return isAndroid ? androidAdaptiveBanner : iosAdaptiveBanner;
            case AdType.Interstitial: return isAndroid ? androidInterstitial : iosInterstitial;
            case AdType.Rewarded: return isAndroid ? androidRewarded : iosRewarded;
            case AdType.RewardedInterstitial: return isAndroid ? androidRewardedInterstitial : iosRewardedInterstitial;
            case AdType.Native: return isAndroid ? androidNative : iosNative;
            default: return string.Empty;
        }
    }
}
public enum AdType
{
    AppOpen,
    Banner,
    AdaptiveBanner,
    Interstitial,
    Rewarded,
    RewardedInterstitial,
    Native
}