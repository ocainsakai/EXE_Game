using _Game.Core.Gameplay;
using _Game.Core.Gameplay.Combat;
using TMPro;
using UnityEngine;

public class UICompleteScreen : BaseScreen
{ // Chỉ WinScreen mới cần MapManager
    [SerializeField] private MapManager mapManager;
    [SerializeField] private BattleManager battleManager;
    [SerializeField] TextMeshProUGUI coinsText;
    
    private void Start()
    {
        battleManager = FindFirstObjectByType<BattleManager>();  
        mapManager = FindFirstObjectByType<MapManager>();
    }
    private void OnEnable()
    {
        coinsText.text = $"{battleManager.enemyReward} coins";
    }
    protected override void OnButton1Clicked()
    {
        PlayerController.instance.Stat.AddGold(battleManager.enemyReward);
        
        SceneLoader.Instance.LoadScene("MainMenu");
    }

    protected override void OnButton2Clicked()
    {
        AdManager.Instance.ShowRewardedAd(OnAdRewardSuccess);
    }
    private void OnAdRewardSuccess()
    {
        PlayerController.instance.Stat.AddGold(battleManager.enemyReward*2);

        Hide(); // Tự đóng lại
    }
}