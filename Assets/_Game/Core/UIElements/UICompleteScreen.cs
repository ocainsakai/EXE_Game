using _Game.Core.Gameplay;
using _Game.Core.Gameplay.Combat;
using UnityEngine;

public class UICompleteScreen : BaseScreen
{ // Chỉ WinScreen mới cần MapManager
    [SerializeField] private MapManager mapManager;
    [SerializeField] private BattleManager battleManager;
    private void Start()
    {
        battleManager = FindFirstObjectByType<BattleManager>();  
        mapManager = FindFirstObjectByType<MapManager>();
    }
    protected override void OnButton1Clicked()
    {
        PlayerController.instance.Stat.AddGold(battleManager.enemyReward);
        
        SceneLoader.Instance.LoadScene("MainMenu");
    }

    protected override void OnButton2Clicked()
    {
        Debug.Log("Watching Ad for x2 reward...");
        
        // Bạn nên dùng callback của AdManager để đảm bảo xem xong Ad
        AdManager.Instance.ShowRewardedAd(OnAdRewardSuccess);
    }
    private void OnAdRewardSuccess()
    {
        // Logic này chỉ chạy KHI xem Ad thành công
        Debug.Log("Ad success! Claiming x2 reward...");
        // Ví dụ: SaveData.Instance.AddGold(200);
        PlayerController.instance.Stat.AddGold(battleManager.enemyReward*2);

        Hide(); // Tự đóng lại
    }
}