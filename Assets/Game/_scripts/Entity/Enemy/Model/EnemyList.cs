using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyList", menuName = "Scriptable Objects/EnemyList")]
public class EnemyList : ScriptableObject
{
    public List<Enemy> enemies;
}
