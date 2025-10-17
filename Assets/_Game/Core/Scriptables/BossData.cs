using UnityEngine;

[CreateAssetMenu(fileName = "BossData", menuName = "Scriptable Objects/BossData")]
public class BossData : EnemyData
{
    public string BossID => enemyID;
}
