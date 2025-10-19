using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyData")]
public class EnemyData : ScriptableObject
{
    public string enemyID;
    public string nameDisplay;
    [FormerlySerializedAs("Icon")] public Sprite icon;
    [FormerlySerializedAs("Atk")] public int atk;
    [FormerlySerializedAs("HP")] public int hp;
    [FormerlySerializedAs("Count")] public int count;
    public int cost;
    public int reward;
}