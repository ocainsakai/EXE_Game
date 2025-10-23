using UnityEngine;
using UnityEngine.UI;
using TMPro; // Thêm dòng này nếu bạn dùng TextMeshPro

public class MapEntry : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI mapNameText;
    [SerializeField] private Image mapBackground;

    public void SetMapName(string name)
    {
        if (mapNameText != null)
        {
            mapNameText.text = name;
        }
    }

    /// <summary>
    /// Đặt hình nền cho Map.
    /// </summary>
    public void SetMapBackground(Sprite background)
    {
        if (mapBackground != null)
        {
            mapBackground.sprite = background;

            mapBackground.gameObject.SetActive(background != null);
        }
    }
}
