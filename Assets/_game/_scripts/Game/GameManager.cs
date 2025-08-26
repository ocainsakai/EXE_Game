using Map;
using System;
using UnityEngine.SceneManagement;

public class GameManager : GlobalSingleton<GameManager>
{
    public SceneLoader sceneLoader;
    public InputManager inputManager;
    public EnemyData enemies;
    public PlayerConfig playerConfig;
    public void ChangeScenceToCombat()
    {
        sceneLoader.LoadScene( () => SceneManager.LoadScene("Battle")); 
    }

    internal void ChangeScenceToMap()
    {
        SceneManager.LoadScene("Map");
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