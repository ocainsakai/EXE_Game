using Ain;
using System.Linq;

public class PlayerAttackPhase : IState
{
    private PlayerHand handController;
    private EnemyList enemyList;

    public PlayerAttackPhase(PlayerHand handController, EnemyList enemyList)
    {
        this.handController = handController;
        this.enemyList = enemyList;
    }

    public void OnEnter()
    {
        foreach (var card in handController.TakeSelected())
        {
            // Assuming each card has a method to attack an enemy
            var targetEnemy = enemyList.enemies.FirstOrDefault();
            if (targetEnemy != null)
            {
                card.Attack(targetEnemy);
            }
        }
    }

    public void OnExit()
    {
        // Reset the hand or perform any cleanup if necessary
        handController.ClearSelected();
    }

    public void Tick()
    {
    }
}