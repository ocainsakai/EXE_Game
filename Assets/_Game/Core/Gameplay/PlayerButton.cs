using System.Collections.Generic;
using BulletHellTemplate;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerButton : MonoBehaviour
{
    // Using a Dictionary allows you to access buttons by name, which is more robust.
    private Dictionary<string, Button> actionButtons = new Dictionary<string, Button>();

    [SerializeField] private AudioClip btnClick;
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
            new { Name = "Đánh",    Action = new UnityAction(onPlayButtonClicked.Invoke) },
            new { Name = "Bỏ bài", Action = new UnityAction(onDiscardButtonClicked.Invoke) },
            new { Name = "Sắp xếp",    Action = new UnityAction(onSortButtonClicked.Invoke) },
            new { Name = "Kết thúc lượt",     Action = new UnityAction(onEndTurntButtonClicked.Invoke) }
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
        var newButtonGo = new GameObject(title + "Button", typeof(RectTransform));
        newButtonGo.transform.SetParent(this.transform, false); // Set 'worldPositionStays' to false
        newButtonGo.transform.localScale = Vector3.one;

        // 
       
        
        // Add Button and Image components
        var image = newButtonGo.AddComponent<Image>();
        image.color = Color.gray;

        var btn = newButtonGo.AddComponent<Button>();
        // Directly add the provided action as a listener. This is the key change.
        if (onClickAction != null)
        {
            btn.onClick.AddListener(onClickAction);
        }
        var btnAudio = btn.gameObject.AddComponent<ButtonAudio>();
        btnAudio.audioTag = "master";
        btnAudio.buttonClickAudio = btnClick;
        // Create the text object
        var textGo = new GameObject("Text", typeof(RectTransform));
        textGo.transform.SetParent(newButtonGo.transform, false);
        textGo.transform.localScale = Vector3.one;

        // Add and configure TextMeshPro component
        var textContent = textGo.AddComponent<TextMeshProUGUI>();
        textContent.text = title;
        textContent.alignment = TextAlignmentOptions.Center;
        textContent.fontSize = 24;
        textContent.color = Color.white;

        // Anchor the text to fill the button
        var textRect = textGo.GetComponent<RectTransform>();
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