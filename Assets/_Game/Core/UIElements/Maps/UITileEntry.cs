using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UITileEntry : MonoBehaviour
{
    [FormerlySerializedAs("_background")] [SerializeField] Image background;
    [FormerlySerializedAs("_content")] [SerializeField] Image content;
    [FormerlySerializedAs("_button")] [SerializeField] Button button;
    public static UnityAction<Vector2Int> OnTileMapClicked;
    private Vector2Int _position;
    public void SetData(Vector2Int position,Sprite icon, Color bgColor)
    {
        _position = position;
        if (icon != null) content.sprite = icon;
        else content.gameObject.SetActive(false);
        background.color = bgColor;

        button.onClick.AddListener(() => OnTileMapClicked?.Invoke(_position));
    }

    void OnDestroy()
    {
        button.onClick.RemoveAllListeners();
    }
}
