
using UnityEngine;

public class PlayerAction : PlayerBaseState
{
    public PlayerAction(PlayerController controller) : base(controller)
    {
    }

    public override void OnEnter()
    {
        Debug.Log("PlayerAction: OnEnter");
        if (EnemyManager.Instance.enemyList.enemies == null ||
            EnemyManager.Instance.enemyList.enemies.Count == 0)
        {
            Debug.LogWarning("No enemies found in the database.");
            BattleManager.Instance.WinBattle();
            return;
        }
        controller.EnableAllActions();
        // Logic for entering the action phase, if any
    }
    public override void OnExit()
    {
        Debug.Log("PlayerAction: OnExit");
        controller.DisableAllActions();
        // Logic for exiting the action phase, if any
    }

    public override void Tick()
    {
       
    }
}