using Ain;
using System.Collections.Generic;
using UnityEngine;
public class EnemyStateMachine : StateMachine<EnemyState, IState>
{
    public EnemyStateMachine(Enemy enemy, Animator animator, Counter counter)
    {
        States = new Dictionary<EnemyState, IState>{
            { EnemyState.Idle, new EnemyIdle(animator) } ,
            { EnemyState.Counting, new EnemyCounting(counter) } ,
            { EnemyState.Hurt, new EnemyHurt(enemy, animator) } ,
            { EnemyState.Dead, new EnemyDead(enemy, animator) } ,
            { EnemyState.Attack, new EnemyAttack(enemy,animator) } ,
        };

    }
}

