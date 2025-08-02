using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDatabase", menuName = "Scriptable Objects/EnemyDatabase")]
public class EnemyDatabase : ScriptableObject
{
    public List<EnemyData> Enemies;

}
[System.Serializable]
public class EnemyData
{
    public string EnemyID;
    public string DisplayName;
    public Sprite Icon;
    public int BaseHP;
    public int BaseAttack;
    public GameObject Prefab; // Prefab chứa EnemyEntity
    public List<CardData> Deck;
}