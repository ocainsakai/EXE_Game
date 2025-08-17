using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    public string DisplayName;
    public Sprite Icon;
    public int Atk;
    public int HP;
    public int Count;
    public int cost;
    public int reward;
}