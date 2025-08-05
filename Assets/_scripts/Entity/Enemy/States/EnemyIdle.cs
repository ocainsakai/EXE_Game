using Ain;
using TMPro;
using UnityEngine;

public class EnemyIdle : IState
{
    private Animator _animator;

    public EnemyIdle(Animator animator)
    {
        _animator = animator;
    }

    public void OnEnter()
    {
        _animator.SetBool("Idle", true);
    }

    public void OnExit()
    {
        _animator.SetBool("Idle", false);

    }

    public void Tick()
    {
    }
}

