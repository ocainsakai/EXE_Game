using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyBattleUI : MonoBehaviour
{
    [SerializeField] Transform slotPrf;  
    public List<Enemy> InitializedEnemy(IEnumerable<Enemy> enemies)
    {
        var enemyList = new List<Enemy>();  
        if (enemies == null || enemies.Count() == 0)
        {
            Debug.Log("Error here");
            return enemyList;
        }
        foreach (var enemyData in enemies)
        {
            var slot = Instantiate(slotPrf);
            var enemyEntity = Instantiate(enemyData);
            slot.SetParent(transform);
            enemyEntity.transform.SetParent(slot);
            enemyEntity.transform.localPosition = Vector3.zero;
            enemyList.Add(enemyEntity);
        }
        return enemyList;
    }
}
