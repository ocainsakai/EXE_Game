using System;
using _Game.Core.Gameplay.Combat;
using _Game.Core.UIElements;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] MapManager mapManager;
    [SerializeField] BattleManager battleManager;

    [SerializeField] private GameObject mapUI;
    [SerializeField] private UIScreens uiScreens;

    public static GameManager instance;
    public void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public void Start()
    {
        InitGame();
    }

    void InitGame()
    {
        mapUI.SetActive(true);
        uiScreens.CloseAllScreens();         
    }
}
