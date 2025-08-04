using Ain;
using System.Linq;

public class PlayerAttackPhase : IState
{
    //private PlayerController playerController;
    private PlayerController _player;
    private EnemyList enemyList;

    public PlayerAttackPhase(PlayerController handController, EnemyList enemyList)
    {
        this._player = handController;
        this.enemyList = enemyList;
    }

    public void OnEnter()
    {
        int mult = _player.handController.Mult;
        foreach (var card in _player.handController.TakeSelected())
        {
            // Assuming each card has a method to attack an enemy
            var targetEnemy = enemyList.enemies.FirstOrDefault(x => !x.IsDead.Value);
            if (targetEnemy != null)
            {
                card.Attack(targetEnemy, mult);
            }
        }
        _player.EndTurn();
    }

    public void OnExit()
    {
        // Reset the hand or perform any cleanup if necessary
        _player.handController.Discard(_player.handController.Hand);
    }

    public void Tick()
    {
    }
}