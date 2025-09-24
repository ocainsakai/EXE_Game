using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeckEntry : MonoBehaviour
{
    [SerializeField] private Image cardBack;
    [SerializeField] private TextMeshProUGUI deckNameText;
    public void Awake()
    {
        if (cardBack == null)
            cardBack = GetComponentInChildren<Image>();
        if (deckNameText == null)
        {
            deckNameText = GetComponentInChildren<TextMeshProUGUI>();

            deckNameText.text = "No Name";
        }
    }

    public void SetDeckName(string name)
    {
        deckNameText.text = name;
    }
    public void SetCardBack(Sprite sprite)
    {
        cardBack.sprite = sprite;
    }
}
