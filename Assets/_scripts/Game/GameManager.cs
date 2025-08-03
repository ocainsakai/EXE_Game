using Ain;

using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
   
    public InputManager inputManager;


    public void ChangeScenceToCombat()
    {
        SceneManager.LoadScene("Battle");
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