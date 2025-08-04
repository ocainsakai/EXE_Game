using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    public Sprite Icon;
    public int cost;
    public int reward;
    public GameObject Prefab; 
}