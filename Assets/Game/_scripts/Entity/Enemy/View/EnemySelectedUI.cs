using UnityEngine;
using UnityEngine.UI;

public class EnemySelectedUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Transform[] slots;

    [SerializeField] private EnemyDatabase combatContext;

    private int currentSlotIndex = -1;

    private void Start()
    {
        playButton.onClick.AddListener(() => GameManager.Instance.ChangeScenceToCombat());
        if (slots.Length == 0)
        {
            Debug.LogError("No slots assigned for enemy selection.");
            return;
        }
        combatContext.Clear();
        currentSlotIndex = -1;
    }
    public void AddEnemySelected(Enemy enemyData)
    {
        if (enemyData == null)
        {
            Debug.Log("error here");
            return;
        }
        currentSlotIndex++;
        if (currentSlotIndex >= slots.Length)
        {
            Debug.LogWarning("No more slots available for enemy selection.");
            return;
        }
        combatContext.Enemies.Add(enemyData);
        Transform slot = slots[currentSlotIndex];
        var enemyEntity = Instantiate(enemyData);
        enemyEntity.transform.localPosition = Vector3.zero;
        enemyEntity.transform.SetParent(slot, false);
    }
}
