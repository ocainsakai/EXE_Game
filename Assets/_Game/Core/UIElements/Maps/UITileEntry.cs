using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UITileEntry : MonoBehaviour
{
    [SerializeField] Image _background;
    [SerializeField] Image _content;
    [SerializeField] Button _button;
    public static UnityAction<Vector2Int> OnTileMapClicked;
    private Vector2Int _position;
    public void SetData(Vector2Int position,Sprite icon, Color bgColor)
    {
        _position = position;
        _content.sprite = icon;
        _background.color = bgColor;

        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(() => OnTileMapClicked?.Invoke(_position));
    }

    void OnDestroy()
    {
        _button.onClick.RemoveAllListeners();
    }
}
