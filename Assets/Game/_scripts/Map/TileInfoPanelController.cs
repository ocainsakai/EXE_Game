using UnityEngine;
using TMPro;

public class TileInfoPanelController : MonoBehaviour
{
    public static TileInfoPanelController Instance;
    public GameObject panel;
    public TextMeshProUGUI tileTypeText;

    private void Awake()
    {
        Instance = this;
        Hide();
    }

    public void Show(string tileType)
    {
        tileTypeText.text = tileType;
        panel.SetActive(true);
    }

    public void Hide()
    {
        panel.SetActive(false);
    }
}
