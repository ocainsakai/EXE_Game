using System;
using _Game.Core.Gameplay;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerActionController  Action;
    public PlayerStatComponent  Stat;

    public static PlayerController instance;    
    
    private void Awake()
    {
        Action = GetComponent<PlayerActionController>();
        Stat = GetComponent<PlayerStatComponent>();

        instance = this;
    }
}
