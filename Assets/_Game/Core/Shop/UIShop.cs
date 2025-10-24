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
}
