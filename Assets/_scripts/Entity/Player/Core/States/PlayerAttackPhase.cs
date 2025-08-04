using System.Linq;
using UnityEngine;

public class PlayerAttackPhase : PlayerBaseState
{
    private EnemyList enemyList;
    public PlayerAttackPhase(PlayerController handController) : base(handController)
    {
        enemyList = EnemyManager.Instance.enemyList;
    }

    public override void OnEnter()
    {
        Debug.Log("PlayerAttackPhase: OnEnter");
        int mult = controller.handController.Mult;
        foreach (var card in controller.handController.TakeSelected())
        {
            // Assuming each card has a method to attack an enemy
            var targetEnemy = enemyList.enemies.FirstOrDefault(x => !x.IsDead.Value);
            if (targetEnemy != null)
            {
                card.Attack(targetEnemy, mult);
            }
        }
        controller.EndTurn();
    }

    public override void OnExit()
    {
        Debug.Log("PlayerAttackPhase: OnExit");
        // Reset the hand or perform any cleanup if necessary
        controller.handController.Discard(controller.handController.Hand);
    }

    public override void Tick()
    {
    }
}