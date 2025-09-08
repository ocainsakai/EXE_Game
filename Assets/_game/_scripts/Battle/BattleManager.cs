using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
public class BattleManager : MonoBehaviour, ITurnBasedBattleManager<IBattleActor, IBattleAction, IBattleResult>
{

    [SerializeField] private bool isTestMode = false;

    public void EndBattle()
    {
    }

    public void EndTurn()
    {
    }

    public IBattleResult ExecuteAction(IBattleAction action)
    {
        throw new NotImplementedException();
    }

    public IBattleResult GetBattleResult()
    {
        throw new NotImplementedException();
    }

    public void InitializeBattle(IEnumerable<IBattleActor> actors)
    {
        throw new NotImplementedException();
    }

    public bool IsBattleOver()
    {
        throw new NotImplementedException();
    }

    public void StartBattle()
    {
        throw new NotImplementedException();
    }

    public IBattleActor StartTurn()
    {
        throw new NotImplementedException();
    }
}
