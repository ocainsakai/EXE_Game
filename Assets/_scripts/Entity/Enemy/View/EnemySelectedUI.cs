using UnityEngine;

public class EnemySelectedUI : MonoBehaviour
{
    
    [SerializeField] private Transform[] slots;

    [SerializeField] private EnemyDatabase combatContext;

    private int currentSlotIndex = -1;

    private void Start()
    {
        if (slots.Length == 0)
        {
            Debug.LogError("No slots assigned for enemy selection.");
            return;
        }
        combatContext.Clear(); // Clear the combat context to start fresh
        // Initialize enemyDataList with the same length as slots
    }
    public void AddEnemySelected(EnemyData enemyData)
    {
        currentSlotIndex++;
        if (currentSlotIndex >= slots.Length)
        {
            Debug.LogWarning("No more slots available for enemy selection.");
            return;
        }
        combatContext.Enemies.Add(enemyData);
        Transform slot = slots[currentSlotIndex];
        var enemyEntity = Instantiate(enemyData.Prefab);
        enemyEntity.transform.localPosition = Vector3.zero;
        enemyEntity.transform.SetParent(slot, false);
    }
}
