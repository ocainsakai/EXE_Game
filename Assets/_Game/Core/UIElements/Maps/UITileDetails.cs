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

    [Header("Enemy Stats")]
    [SerializeField] GameObject enemyStatsPanel; // Panel chứa thông tin enemy, để dễ ẩn/hiện
    [SerializeField] TextMeshProUGUI hpText;
    [SerializeField] TextMeshProUGUI atkText;
    [SerializeField] TextMeshProUGUI rewardText;

    public UnityEvent<Tile> OnPlayBtnClicked;
    public void Show(Tile tile)
    {
        gameObject.SetActive(true);
        avatar.sprite = tile.Icon;

        // Dùng một biến tạm để lưu dữ liệu, có thể là EnemyData hoặc BossData
        // (Giả định rằng BossData có các trường tương tự EnemyData)
        EnemyData enemyData = null;

        // THAY ĐỔI: Kiểm tra loại Tile và gọi hàm tương ứng từ GameInstance
        switch (tile.Type)
        {
            case TileType.Enemy:
                // Lấy thông tin Enemy từ map hiện tại
                enemyData = GameInstance.Singleton.GetEnemyData(tile.OccupantID);
                break;

            case TileType.Boss:
                // Lấy thông tin Boss từ map hiện tại
                // Chúng ta tạm coi BossData có thể được dùng như EnemyData
                enemyData = GameInstance.Singleton.GetBoss(tile.OccupantID);
                break;
        }

        // Cập nhật UI nếu tìm thấy thông tin
        if (enemyData != null)
        {
            enemyStatsPanel.SetActive(true);
            decription.text = enemyData.Name;
            hpText.text = $"HP: {enemyData.HP}";
            atkText.text = $"ATK: {enemyData.Atk}";
            rewardText.text = $"Reward: {enemyData.reward}"; // Lưu ý: Tên biến có thể khác
        }
        else
        {
            // Nếu là ô trống hoặc loại khác không có dữ liệu
            enemyStatsPanel.SetActive(false);
            tileNameText.text = $"Tile ({tile.Position.x}, {tile.Position.y})";
        }

        // Logic của nút Play không đổi
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
