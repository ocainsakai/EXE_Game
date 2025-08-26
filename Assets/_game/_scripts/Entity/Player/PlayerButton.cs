using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerButton : MonoBehaviour
{
    [SerializeField] private Button[] actionButtons;

    public Action onPlayButtonClicked;
    public Action onDiscardButtonClicked;
    public Action onSortButtonClicked;

    private void Start()
    {
        SetupActionButtons();
    }
    public void SetupActionButtons()
    {
        if (actionButtons == null || actionButtons.Length < 4)
        {
            Debug.LogError("Action buttons not properly assigned!");
            return;
        }

        // Set button texts
        actionButtons[0].GetComponentInChildren<TextMeshProUGUI>().text = "PLAY";
        actionButtons[1].GetComponentInChildren<TextMeshProUGUI>().text = "DISCARD";
        actionButtons[2].GetComponentInChildren<TextMeshProUGUI>().text = "SORT";
        actionButtons[3].GetComponentInChildren<TextMeshProUGUI>().text = "SKIP";

        // Clear existing listeners to prevent duplicates
        foreach (var button in actionButtons)
        {
            button.onClick.RemoveAllListeners();
        }
        Debug.Log("add listener");
        // Add new listeners
        actionButtons[0].onClick.AddListener(() => onPlayButtonClicked?.Invoke());
        actionButtons[1].onClick.AddListener(() => onDiscardButtonClicked?.Invoke());
        actionButtons[2].onClick.AddListener(() => onSortButtonClicked?.Invoke());
    }

    // Action control methods
    #region Action Control Methods
    public void EnableAllActions()
    {
        foreach (var button in actionButtons)
        {
            button.interactable = true;
        }
    }

    public void DisableAllActions()
    {
        foreach (var button in actionButtons)
        {
            button.interactable = false;
        }
    }
    #endregion
}
