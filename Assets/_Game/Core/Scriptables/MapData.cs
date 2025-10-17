using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "MapData", menuName = "Scriptable Objects/MapData")]
public class MapData : ScriptableObject
{
    [FormerlySerializedAs("MapID")] public string mapID;
    [FormerlySerializedAs("MapName")] public string mapName;
    public EnemyData[] enemyDatas;
    public BossData[] bossDatas;
    
    // Compatibility properties để tương thích với code hiện tại
    public EnemyData[] EnemyData => enemyDatas;
    public BossData[] BossData => bossDatas;
}
