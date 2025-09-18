using UnityEngine;
using System;
using Game.Service;
using UnityUtils;

public interface IGameManager
{
    void Init();
    void StartGame();
    void EndGame();
    void QuitGame();
}

public class GameManager : Singleton<GameManager>, IGameManager
{
    public static event Action OnGameStarted;
    public static event Action OnGamePaused;
    public static event Action OnGameOver;

    private StateMachine stateMachine;

    private void Awake()
    {
        stateMachine = new StateMachine();
    }
    private void Start()
    {
        stateMachine.SetState(new InitState());
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void Init()
    {
    }

    public void StartGame()
    {
    }

    public void EndGame()
    {
    }
}
