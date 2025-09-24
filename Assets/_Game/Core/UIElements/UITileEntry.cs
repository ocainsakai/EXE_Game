using UnityEngine;
using UnityEngine.UI;

public class UITileEntry : MonoBehaviour
{
    [SerializeField] Image _background;
    [SerializeField] Image _content;

    public void SetData(Sprite icon, Color bgColor)
    {
        _content.sprite = icon;
        _background.color = bgColor;
    }
}
