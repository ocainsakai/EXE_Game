using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Manager tổng thể cho battle flow - kết nối Map và Battle System
/// </summary>
public class BattleManager : MonoBehaviour
{
    [SerializeField] private Enemy enemy;
    [SerializeField] private PlayerActionController playerActionController;
    [SerializeField] private PlayerStatComponent playerStatComponent;

    public UnityEvent OnEndBattle;
    private void Start()
    {
        
    }

    public void CheckCondition(object sender)
    {
        if (playerStatComponent.HP <= 0)
        {
            Debug.Log($"You lose");
            // lose resolve
            OnEndBattle?.Invoke();
            return;
        }
        if (enemy.HP <=0)
        {
            Debug.Log($"You win");
            // win resolve
            OnEndBattle?.Invoke();
            return;
        }
        if (sender != null && (sender is Enemy))
        {
            Debug.Log($"You start your turn");
            playerActionController.PlayerStartTurn();
            return;
        }
        if (sender != null && sender is PlayerActionController)
        {
            Debug.Log($"You start enemy turn");
            enemy.CountToAction();
            return;
        }
    }
}