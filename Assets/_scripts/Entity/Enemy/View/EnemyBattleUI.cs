using System.Collections.Generic;
using UnityEngine;

public class EnemyBattleUI : MonoBehaviour
{
    private List<Transform> _slots = new List<Transform>();
    private void Awake()
    {
        _slots.Clear();
        foreach (Transform child in transform)
        {
            _slots.Add(child);
        }
        // Initialize any necessary components or references here
    }
    public List<Enemy> InitializedEnemy(IEnumerable<EnemyData> enemies)
    {
        List<Enemy> enemyList = new List<Enemy>();
        int i = 0;
        foreach (var enemyData in enemies)
        {
            Transform slot = _slots[i];
    
            var enemyEntity = Instantiate(enemyData.Prefab);
            enemyEntity.transform.localPosition = Vector3.zero;
            enemyEntity.transform.SetParent(slot, false);
            enemyList.Add(enemyEntity.GetComponent<Enemy>());
            i++;
        }
        return enemyList;
    }
}
