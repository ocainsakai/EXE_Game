using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDatabase", menuName = "Scriptable Objects/EnemyDatabase")]
public class EnemyDatabase : ScriptableObject
{
    public List<EnemyData> Enemies;

    public void Clear()
    {
        Enemies.Clear();
    }
}
