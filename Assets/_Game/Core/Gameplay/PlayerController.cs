using System;
using _Game.Core.Gameplay;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerActionController  Action;
    public PlayerStatComponent  Stat;

    private void Awake()
    {
        Action = GetComponent<PlayerActionController>();
        Stat = GetComponent<PlayerStatComponent>();
    }
}
