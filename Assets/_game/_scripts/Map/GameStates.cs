using Map;
using UnityEngine;

[CreateAssetMenu(fileName = "MapState", menuName = "Scriptable Objects/MapState")]
public class GameStates : ScriptableObject
{
    public bool isInitMap;
    [Header("Map Field")]
    public SerializableDictionary<Vector2Int, HexState> mapStates;
    public Vector2Int playerPostion;
    public Vector2Int lastClickPostion;

    [Header("Battle Field")]
    public EnemyData enemyData;
    public bool lastBattleResult;
}
