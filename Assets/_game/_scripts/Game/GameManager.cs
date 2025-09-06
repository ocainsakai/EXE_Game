using Map;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityUtils;

[DefaultExecutionOrder(-1)]

public class GameManager : Singleton<GameManager>
{
    public SceneLoader sceneLoader;
    public InputManager inputManager;
    public EnemyData enemies;
    public PlayerConfig playerConfig;

    // data cache
    public BattleResult BattleResult;
    // event

    public Action NewGame;
    public Action<BattleResult> OnBattleResult;
    private void Start()
    {
        NewGame?.Invoke();
    }
    public void ChangeScenceToCombat()
    {
        sceneLoader.LoadScene( () => SceneManager.LoadScene("Battle")); 
    }

    public void ChangeScenceToMap()
    {
        SceneManager.sceneLoaded += OnMapSceneLoaded;
        SceneManager.LoadScene("Map");
    }
    private void OnMapSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Map")
        {
            OnBattleResult?.Invoke(BattleResult);
            SceneManager.sceneLoaded -= OnMapSceneLoaded; 
        }
    }
}
