using UnityEngine;
using VContainer;

public class MainMenuController : MonoBehaviour
{
    private IGameManager gameManager;

    [Inject]
    public void Construct(IGameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    private void Start()
    {
        Debug.Log($"[MainMenu] GameManager hash: {gameManager.GetHashCode()}");
        Debug.Log($"[MainMenu] Current State: {gameManager}");
    }
}
