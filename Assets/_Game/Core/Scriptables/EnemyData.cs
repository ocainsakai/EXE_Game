using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyData")]
public class EnemyData : ScriptableObject
{
    [FormerlySerializedAs("EnemyID")] public string enemyID;
    [FormerlySerializedAs("Name")] public string name;
    [FormerlySerializedAs("Icon")] public Sprite icon;
    [FormerlySerializedAs("Atk")] public int atk;
    [FormerlySerializedAs("HP")] public int hp;
    [FormerlySerializedAs("Count")] public int count;
    public int cost;
    public int reward;
}