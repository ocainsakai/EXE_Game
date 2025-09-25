using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UITileDetails : MonoBehaviour
{
    [SerializeField] Button playBtn;
    [SerializeField] Image avatar;
    [SerializeField] TextMeshProUGUI tileNameText;
    [SerializeField] TextMeshProUGUI decription;

    public UnityEvent<Tile> OnPlayBtnClicked;
    public void Show(Tile tile)
    {
        gameObject.SetActive(true);
        // Update UI elements to show tile details
        avatar.sprite = tile.Icon;
        tileNameText.text = $"Tile ({tile.Position.x}, {tile.Position.y})";
        decription.text = tile.Type.ToString();


        playBtn.gameObject.SetActive(tile.IsWalkable);
        playBtn.onClick.RemoveAllListeners();
        playBtn.onClick.AddListener(() => {
            OnPlayBtnClicked?.Invoke(tile);
            Hide();
        });
    }
    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
}
