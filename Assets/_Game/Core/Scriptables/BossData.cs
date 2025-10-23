using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossData", menuName = "Scriptable Objects/BossData")]
public class BossData : EnemyData
{
    public string BossID => enemyID;

    [Header("Boss Abilities")]
    public List<AbilityData> abilities;
}
