using Ain;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class EnemyHurt : IState
{
    private readonly Enemy _enemy;
    private Animator _animator;

    public EnemyHurt(Enemy enemy, Animator animator)
    {
        _enemy = enemy;
        _animator = animator;
    }

    public void OnEnter()
    {
        HurtAsync();
    }
    private async void HurtAsync()
    {
        if (_animator != null)
        {
            _animator.SetTrigger("Hurt");
        }
        await UniTask.Delay(500);
        //_enemy.Check()
    }
    public void OnExit()
    {
    }

    public void Tick()
    {
    }

}

