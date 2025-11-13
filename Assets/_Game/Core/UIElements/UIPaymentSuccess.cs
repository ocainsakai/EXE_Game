using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIPaymentSuccess : MonoBehaviour
{
    [Header("Text Elements")]
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private TextMeshProUGUI rewardText;
    
    [Header("Buttons")]
    [SerializeField] private Button closeButton;

    private void Awake()
    {
        if (closeButton != null)
            closeButton.onClick.AddListener(OnCloseClicked);
        
        // Ẩn ban đầu
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Hiển thị popup thanh toán thành công
    /// </summary>
    public void Show(string message, string reward)
    {
        if (messageText != null)
            messageText.text = message;
        
        if (rewardText != null)
            rewardText.text = reward;
        
        gameObject.SetActive(true);
    }

    private void OnCloseClicked()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Ẩn popup
    /// </summary>
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}