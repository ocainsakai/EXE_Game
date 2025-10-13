using UnityEngine;

[CreateAssetMenu(fileName = "MapData", menuName = "Scriptable Objects/MapData")]
public class MapData : ScriptableObject
{
    public string MapID;
    public string MapName;
    public EnemyData[] enemyDatas;
    public BossData[] bossDatas;
    
    // Compatibility properties để tương thích với code hiện tại
    public EnemyData[] enemyData => enemyDatas;
    public BossData[] bossData => bossDatas;
}
