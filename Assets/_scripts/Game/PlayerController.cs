using Cysharp.Threading.Tasks;
using UnityEngine;
public class PlayerController : Singleton<PlayerController>
{
    [SerializeField] private CardManager cardManager;
    [SerializeField] private HealthSystem healthSystem;
    [SerializeField] private PokerData pokerData;
    [SerializeField] private PlayerStat stat;

    public bool IsDead => stat.Hp.Current.Value <= 0;
    public async UniTask Initialize()
    {
        stat = new()
        {
            Hp = new(300),
            Coin = new(1000),
            Energy = new(10),
        };
        healthSystem.SetStat(stat.Hp);
        await UniTask.Delay(healthSystem.AnimationDuration);
    }
    
    public async UniTask TakeDame(int amount)
    {
        healthSystem.TakeDame(amount);
        await UniTask.Delay(healthSystem.AnimationDuration);
    }
    public async UniTask BattleStart()
    {
        await Initialize();
    }

}