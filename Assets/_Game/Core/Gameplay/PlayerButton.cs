using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerButton : MonoBehaviour
{
    [SerializeField] private List<Button> actionButtons;

    
    public UnityEvent onPlayButtonClicked;
    public UnityEvent onDiscardButtonClicked;
    public UnityEvent onSortButtonClicked;

    private void Start()
    {
        CreateBtns();
    }
    public void CreateBtns()
    {
        var play = CreateButton("Play", () => onPlayButtonClicked?.Invoke());
        var discard = CreateButton("Discard", () => onDiscardButtonClicked?.Invoke());
        var sort = CreateButton("Sort", () => onSortButtonClicked?.Invoke());

        actionButtons.Add(play);
        actionButtons.Add(discard);
        actionButtons.Add(sort);
    }

    private Button CreateButton(string title, Action onClick = null)
    {

        // Create a new GameObject for the button
        var newButton = new GameObject();
        newButton.transform.SetParent(this.transform);
        newButton.name = title + "Button";
        newButton.transform.localScale = Vector3.one;


        // Add Button and Image components
        var btn = newButton.AddComponent<Button>();
        btn.onClick.AddListener(() => onClick?.Invoke());
        var image = newButton.AddComponent<Image>();
        image.color = Color.gray;


        // Add RectTransform and set size
        var text = new GameObject();
        text.transform.SetParent(newButton.transform);
        text.name = "Text";
        text.transform.localScale = Vector3.one;


        // Add TextMeshPro component for button label
        var textContent = text.AddComponent<TextMeshProUGUI>();
        textContent.text = title;
        textContent.alignment = TextAlignmentOptions.Center;
        textContent.fontSize = 24;

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
