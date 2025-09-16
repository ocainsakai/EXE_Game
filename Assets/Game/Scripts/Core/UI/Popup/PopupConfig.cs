using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "PopupConfig", menuName = "UI/PopupConfig")]
public class PopupConfig : ScriptableObject
{
    [Header("Container Settings")]
    public Vector2 size = new Vector2(400, 200);
    public Color backgroundColor = Color.white;

    [Header("Overlay Settings")]
    public Color overlayColor = new Color(0, 0, 0, 0.6f);

    [Header("Text Settings")]
    public int fontSize = 24;
    public Color textColor = Color.black;
    public TextAlignmentOptions textAlignment = TextAlignmentOptions.Center;

    [Header("Button Settings")]
    public Vector2 buttonSize = new Vector2(120, 40);
    public Color buttonBackgroundColor = new Color(0.2f, 0.5f, 0.9f);
    public Color buttonTextColor = Color.white;
    public int buttonFontSize = 20;

    [Header("Popup Type")]
    public bool hasCancelButton = false; // Nếu muốn MessageBox có Cancel
}
