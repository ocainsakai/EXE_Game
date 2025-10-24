using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    [Header("Page")]
    public GameObject soundPage;
    public GameObject infoPage;
    public GameObject creditPage;

    [Header("Texts")]
    public TMP_Text soundText;
    public TMP_Text infoText;
    public TMP_Text creditText;

    [Header("Menu Buttons (optional highlight)")]
    public Button soundButton;
    public Button infoButton;
    public Button creditButton;

    void Start()
    {
        soundButton.onClick.AddListener(ShowSoundPage);
        infoButton.onClick.AddListener(ShowInfoPage);
        creditButton.onClick.AddListener(ShowCreditPage);
        ShowSoundPage();
    }

    private void OnDestroy()
    {
        soundButton.onClick.RemoveListener(ShowSoundPage);
        infoButton.onClick.RemoveListener(ShowInfoPage);
        creditButton.onClick.RemoveListener(ShowCreditPage);
        
    }

    public void ShowSoundPage()
    {
        ResetAll();
        soundPage.SetActive(true);
        infoPage.SetActive(false);
        creditPage.SetActive(false);

        Highlight(soundButton, soundText);
    }

    public void ShowInfoPage()
    {
        ResetAll();
        soundPage.SetActive(false);
        infoPage.SetActive(true);
        creditPage.SetActive(false);

        Highlight(infoButton, infoText);
    }

    public void ShowCreditPage()
    {
        ResetAll();
        soundPage.SetActive(false);
        infoPage.SetActive(false);
        creditPage.SetActive(true);

        Highlight(creditButton, creditText);
    }

    private void ResetAll()
    {
        soundText.color = Color.black;
        soundText.fontStyle = FontStyles.Normal;

        infoText.color = Color.black;
        infoText.fontStyle = FontStyles.Normal;

        creditText.color = Color.black;
        creditText.fontStyle = FontStyles.Normal;

        soundButton.transform.localScale = Vector3.one;
        infoButton.transform.localScale = Vector3.one;
        creditButton.transform.localScale = Vector3.one;
    }
    private void Highlight(Button btn, TMP_Text txt)
    {
        txt.color = Color.white;
        btn.transform.localScale = Vector3.one * 1.1f;
    }
}
