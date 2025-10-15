using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerButton : MonoBehaviour
{
    // Using a Dictionary allows you to access buttons by name, which is more robust.
    private Dictionary<string, Button> actionButtons = new Dictionary<string, Button>();

    public UnityEvent onPlayButtonClicked;
    public UnityEvent onDiscardButtonClicked;
    public UnityEvent onSortButtonClicked;
    public UnityEvent onEndTurntButtonClicked;

    private void Start()
    {
        CreateBtns();
    }

    /// <summary>
    /// Creates the set of action buttons. This method is now "idempotent",
    /// meaning you can call it multiple times without creating duplicate buttons.
    /// </summary>
    public void CreateBtns()
    {
        // 1. Clean up any existing buttons before creating new ones.
        // This prevents duplicates if this function is ever called more than once.
        foreach (var button in actionButtons.Values)
        {
            if (button != null)
            {
                Destroy(button.gameObject);
            }
        }
        actionButtons.Clear();

        // 2. A data-driven approach makes the code cleaner and easier to extend.
        // We pair the button name with the event it should trigger.
        var buttonData = new[]
        {
            new { Name = "Play",    Action = new UnityAction(onPlayButtonClicked.Invoke) },
            new { Name = "Discard", Action = new UnityAction(onDiscardButtonClicked.Invoke) },
            new { Name = "Sort",    Action = new UnityAction(onSortButtonClicked.Invoke) },
            new { Name = "End",     Action = new UnityAction(onEndTurntButtonClicked.Invoke) }
        };

        // 3. Loop through the data to create and configure each button.
        foreach (var data in buttonData)
        {
            Button newButton = CreateButton(data.Name, data.Action);
            actionButtons.Add(data.Name, newButton); // Add to the dictionary.
        }
    }

    /// <summary>
    /// Creates a single button GameObject.
    /// </summary>
    /// <param name="title">The text displayed on the button.</param>
    /// <param name="onClickAction">The action to execute when the button is clicked.</param>
    private Button CreateButton(string title, UnityAction onClickAction)
    {
        // Create a new GameObject for the button
        var newButtonGO = new GameObject(title + "Button", typeof(RectTransform));
        newButtonGO.transform.SetParent(this.transform, false); // Set 'worldPositionStays' to false
        newButtonGO.transform.localScale = Vector3.one;

        // Add Button and Image components
        var image = newButtonGO.AddComponent<Image>();
        image.color = Color.gray;

        var btn = newButtonGO.AddComponent<Button>();
        // Directly add the provided action as a listener. This is the key change.
        if (onClickAction != null)
        {
            btn.onClick.AddListener(onClickAction);
        }

        // Create the text object
        var textGO = new GameObject("Text", typeof(RectTransform));
        textGO.transform.SetParent(newButtonGO.transform, false);
        textGO.transform.localScale = Vector3.one;

        // Add and configure TextMeshPro component
        var textContent = textGO.AddComponent<TextMeshProUGUI>();
        textContent.text = title;
        textContent.alignment = TextAlignmentOptions.Center;
        textContent.fontSize = 24;
        textContent.color = Color.white;

        // Anchor the text to fill the button
        var textRect = textGO.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.sizeDelta = Vector2.zero;

        return btn;
    }

    // region Action Control Methods
    #region Action Control Methods
    public void EnableAllActions()
    {
        foreach (var button in actionButtons.Values)
        {
            button.interactable = true;
        }
    }

    public void DisableAllActions()
    {
        foreach (var button in actionButtons.Values)
        {
            button.interactable = false;
        }
    }
    #endregion
}