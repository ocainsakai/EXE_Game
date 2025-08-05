using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Linq;
using UnityEngine;

public class PlayerAttackPhase : PlayerBaseState
{
    public PlayerAttackPhase(PlayerController handController) : base(handController)
    {
        
    }

    public override void OnEnter()
    {
        Debug.Log("PlayerAttackPhase: OnEnter");
        _ = HandleAttack();
    }

    public override void OnExit()
    {
        Debug.Log("PlayerAttackPhase: OnExit");
        // Reset the hand or perform any cleanup if necessary
        controller.handController.Discard(controller.handController.Hand);
    }

    public override void Tick()
    {
    }
    public async UniTask HandleAttack()
    {
        var sequence = DOTween.Sequence();
        int mult = controller.handController.Mult;
        var attackCards = controller.handController.TakeSelected();

        foreach (var card in attackCards)
        {
            var targetEnemy = EnemyManager.Instance.enemyList.enemies.FirstOrDefault(x => !x.IsDead.Value);
            if (targetEnemy == null) continue;

            await card.transform.DOMoveY(card.transform.position.y + 100, 0.25f).SetEase(Ease.OutQuad)
            .AsyncWaitForCompletion();
            card.Attack(targetEnemy, mult);

            await UniTask.Delay(700);
        }

        await UniTask.Delay(300);
        controller.EndTurn();
    }
}