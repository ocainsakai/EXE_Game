using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIIntrustion : MonoBehaviour
{
 [Header("Panels")]
    [SerializeField] private GameObject tutorialPanel; // Panel chính
    [SerializeField] private Sprite[] sections; // Các section content
    
    [Header("Navigation Buttons")]
    [SerializeField] private Button prevButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button closeButton;
    
    [Header("Optional - Page Indicator")]
    [SerializeField] private Image content; // Text hiển thị "1/4"
    
    private int currentIndex = 0;

    private void Awake()
    {
        // Setup button listeners
        if (prevButton != null)
            prevButton.onClick.AddListener(OnPrevClicked);
        
        if (nextButton != null)
            nextButton.onClick.AddListener(OnNextClicked);
        
        if (closeButton != null)
            closeButton.onClick.AddListener(OnCloseClicked);
        
        // Hide panel initially
        if (tutorialPanel != null)
            tutorialPanel.SetActive(false);
    }

    /// <summary>
    /// Hiển thị tutorial
    /// </summary>
    public void Show()
    {
        if (tutorialPanel != null)
            tutorialPanel.SetActive(true);
        
        currentIndex = 0;
        ShowSection(currentIndex);
    }

    /// <summary>
    /// Ẩn tutorial
    /// </summary>
    public void Hide()
    {
        if (tutorialPanel != null)
            tutorialPanel.SetActive(false);
    }

    /// <summary>
    /// Hiển thị section theo index
    /// </summary>
    private void ShowSection(int index)
    {
        if (sections == null || sections.Length == 0)
            return;
        
        // Clamp index
        currentIndex = Mathf.Clamp(index, 0, sections.Length - 1);
        content.sprite = sections[currentIndex];
        
        // Update buttons
        UpdateNavigationButtons();
    }

    /// <summary>
    /// Update trạng thái của Prev/Next buttons
    /// </summary>
    private void UpdateNavigationButtons()
    {
        if (prevButton != null)
            prevButton.interactable = currentIndex > 0;
        
        if (nextButton != null)
            nextButton.interactable = currentIndex < sections.Length - 1;
    }

    #region Button Callbacks

    private void OnPrevClicked()
    {
        if (currentIndex > 0)
        {
            ShowSection(currentIndex - 1);
        }
    }

    private void OnNextClicked()
    {
        if (currentIndex < sections.Length - 1)
        {
            ShowSection(currentIndex + 1);
        }
    }

    private void OnCloseClicked()
    {
        Hide();
    }

    #endregion

    private void OnDestroy()
    {
        // Cleanup
        if (prevButton != null)
            prevButton.onClick.RemoveListener(OnPrevClicked);
        
        if (nextButton != null)
            nextButton.onClick.RemoveListener(OnNextClicked);
        
        if (closeButton != null)
            closeButton.onClick.RemoveListener(OnCloseClicked);
    }
}
