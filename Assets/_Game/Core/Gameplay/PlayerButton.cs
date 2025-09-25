using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerButton : MonoBehaviour
{
    [SerializeField] private Button[] actionButtons = new Button[3];

    
    public UnityEvent onPlayButtonClicked;
    public UnityEvent onDiscardButtonClicked;
    public UnityEvent onSortButtonClicked;

    private void Start()
    {
        SetupActionButtons();
    }
    public void SetupActionButtons()
    {
        actionButtons[0] = CreateButton("Play", () => onPlayButtonClicked?.Invoke());
        actionButtons[1] = CreateButton("Discard", () => onDiscardButtonClicked?.Invoke());
        actionButtons[2] = CreateButton("Sort", () => onSortButtonClicked?.Invoke());
    }

    private Button CreateButton(string title, Action onClick = null)
    {
        var newButton = new GameObject();
        newButton.transform.SetParent(this.transform);
        var btn = newButton.AddComponent<Button>();
        btn.onClick.AddListener(() => onClick?.Invoke());
        newButton.AddComponent<Image>();
        var text = new GameObject();
        text.transform.SetParent(newButton.transform);
        var textContent = text.AddComponent<TextMeshProUGUI>();
        textContent.text = title;

        return btn;
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
