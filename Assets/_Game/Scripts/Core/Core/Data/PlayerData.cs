using UnityEngine;

public class PlayerData : MonoBehaviour
{
    [SerializeField] private PlayerConfig playerConfig;
    [SerializeField] private EnemyDatabase enemyDatabase;
    public PlayerConfig PlayerConfig => playerConfig;
    public EnemyDatabase EnemyDatabase => enemyDatabase;
}
