using UnityEngine;
[System.Serializable]
[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyData")]
public class EnemyData : ScriptableObject
{
    public string EnemyID;
    public string Name;
    public Sprite Icon;
    public int Atk;
    public int HP;
    public int Count;
    public int cost;
    public int reward;
}