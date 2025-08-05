using Ain;
using System;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDead : IState
{
    private readonly Enemy _enemy;
    private Animator _animator;

    public EnemyDead(Enemy enemy, Animator animator)
    {
        _enemy = enemy;
        _animator = animator;
    }

    public void OnEnter()
    {
        _enemy.IsDead.Value = true;
        if (_animator != null)
        {
            _animator.SetBool("Dead", _enemy.IsDead.Value);
        }

        try
        {

            if (_animator != null)
            {
                _animator.enabled = false;
            }
            _enemy.GetComponentInChildren<Image>().color = Color.black;

        }
        catch (OperationCanceledException)
        {
            Debug.Log("Enemy destroyed before death animation finished");
        }
    }

    public void OnExit()
    {
        
    }

    public void Tick()
    {
    }
}

