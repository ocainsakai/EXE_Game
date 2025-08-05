using Ain;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class EnemyAttack : IState
{
    private Animator _animator;
    private Enemy _enemy;
    public EnemyAttack(Enemy enemy,Animator animator)
    {
        _animator = animator;
        _enemy = enemy;
    }

    public void OnEnter()
    {
        Attack();
    }
    private async void Attack()
    {
        _animator.SetTrigger("Attack");
        await UniTask.Delay(500);
        PlayerController.Instance.PlayerHealth.TakeDamage(10, DamageType.Physical);
    }

    public void OnExit()
    {
    }

    public void Tick()
    {
    }
}

