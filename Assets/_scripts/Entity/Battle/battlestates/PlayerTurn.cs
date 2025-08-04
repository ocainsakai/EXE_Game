using Ain;
using UnityEngine;

public class PlayerTurn : IState
{
    private BattleManager battleManager;
    private PlayerController playerController;
    public PlayerTurn(BattleManager battleManager, PlayerController playerController)
    {
        this.battleManager = battleManager;
        this.playerController = playerController;
    }

    public void OnEnter()
    {
         Debug.Log("PlayerTurn: OnEnter");
        playerController.StartTurn();
    }

    public void OnExit()
    {
        Debug.Log("PlayerTurn: OnExit");
        playerController.DisableAllActions();
        // Reset the hand or perform any cleanup if necessary
        //playerController.handController.Discard(playerController.handController.Hand);
        //battleManager.CheckCondition();
    }

    public void Tick()
    {
    }
}
