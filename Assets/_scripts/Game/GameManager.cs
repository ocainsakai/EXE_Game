using Ain;
using System;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
   
    public InputManager inputManager;


    public void ChangeScenceToCombat()
    {
        SceneManager.LoadScene("Battle");
    }

    internal void BattleEnd()
    {
        SceneManager.LoadScene("Map_Demo");
    }
}
public enum Phase
{
    None,
    StartTurn,
    PlayerTurn,
    EnemiesTurn,
    EndTurn,
}