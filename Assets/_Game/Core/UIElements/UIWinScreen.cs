using System;
using _Game.Core.Gameplay.Combat;
using UnityEngine;

public class UIWinScreen : BaseScreen
{
    // Chỉ WinScreen mới cần MapManager
    [SerializeField] private MapManager mapManager;
    [SerializeField] private BattleManager battleManager;
    private void Start()
    {
        battleManager = FindFirstObjectByType<BattleManager>();  
        mapManager = FindFirstObjectByType<MapManager>();
    }

    protected override void OnButton1Clicked()
    {
        // --- BÂY GIỜ BẠN CÓ THỂ THÊM LOGIC VÀO ĐÂY ---
        Debug.Log("Claiming normal reward...");
        // Ví dụ: SaveData.Instance.AddGold(100);
        PlayerController.instance.Stat.AddGold(battleManager.enemyReward);
        mapManager.OnBattleWin();
        Hide(); // Tự đóng lại
    }

    protected override void OnButton2Clicked()
    {
        // --- BÂY GIỜ BẠN CÓ THỂ THÊM LOGIC VÀO ĐÂY ---
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

        mapManager.OnBattleWin();
        Hide(); // Tự đóng lại
    }
    
    // Không dùng nút 3 thì cứ để trống
    // protected override void OnButton3Clicked() { }
}