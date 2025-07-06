using UniRx;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GamePhase gamePhase;
    public InputManager inputManager;

    private void Awake()
    {
        inputManager.Play.Subscribe(_ => ChangeTurn(Phase.PlayerTurn));

    }
    private void Start()
    {
        ChangeTurn(Phase.StartTurn);
    }
    public void ChangeTurn(Phase phase)
    {
        gamePhase.currentPhase.Value = phase;
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