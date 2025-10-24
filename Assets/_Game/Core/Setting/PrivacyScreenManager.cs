using UnityEngine;
using TMPro; // Dùng TextMeshPro
using Cysharp.Threading.Tasks; // Import UniTask
using UnityEngine.Networking; // Import UnityWebRequest

public class PrivacyScreenManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI policyTextComponent;
    
    [SerializeField]
    private GameObject loadingIndicator; // Một icon "Loading..."
    
    [SerializeField] 
    private string privacyPolicyURL = "https://your-website.com/privacy.txt";
    
    private const string CACHE_KEY = "PrivacyPolicyCache"; // Key để lưu cache
    public void OpenPrivacyPolicy()
    {
        if (string.IsNullOrEmpty(privacyPolicyURL))
        {
            Debug.LogError("Privacy Policy URL is not set!");
            return;
        }

        // Hàm này sẽ mở trình duyệt mặc định của thiết bị
        Application.OpenURL(privacyPolicyURL);
    }
    /*
    async void OnEnable()
    {
        // 1. Hiển thị text đã cache (nếu có) ngay lập tức
        string cachedText = PlayerPrefs.GetString(CACHE_KEY);
        if (!string.IsNullOrEmpty(cachedText))
        {
            policyTextComponent.text = cachedText;
        }
        else
        {
            // Nếu không có cache, hiển thị "Loading..."
            policyTextComponent.text = "";
            loadingIndicator?.SetActive(true);
        }

        // 2. Luôn luôn tải phiên bản mới nhất từ server
        await LoadPolicyFromWeb();
    }
    */

    private async UniTask LoadPolicyFromWeb()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(privacyPolicyURL))
        {
            try
            {
                await request.SendWebRequest(); // Chờ tải xong

                if (request.result == UnityWebRequest.Result.Success)
                {
                    // Tải thành công
                    string newText = request.downloadHandler.text;
                    policyTextComponent.text = newText;
                    
                    // Lưu vào cache
                    PlayerPrefs.SetString(CACHE_KEY, newText);
                }
                else
                {
                    // Lỗi (server 404, không có mạng...)
                    if (string.IsNullOrEmpty(PlayerPrefs.GetString(CACHE_KEY)))
                    {
                        // Nếu cache cũng rỗng -> hiển thị lỗi
                        policyTextComponent.text = "Failed to load Privacy Policy.\nPlease check your internet connection and try again.";
                    }
                    // Nếu cache không rỗng, ta cứ dùng cache cũ (đã hiển thị ở OnEnable)
                }
            }
            catch (System.Exception e)
            {
                // Lỗi (ví dụ: mất kết nối đột ngột)
                Debug.LogError("Error fetching privacy policy: " + e.Message);
                if (string.IsNullOrEmpty(PlayerPrefs.GetString(CACHE_KEY)))
                {
                    policyTextComponent.text = "An error occurred while loading. Please try again later.";
                }
            }
            finally
            {
                // Dù thành công hay thất bại, tắt icon "Loading..."
                loadingIndicator.SetActive(false);
            }
        }
    }
}