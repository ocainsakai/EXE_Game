using BulletHellTemplate;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.XR;

namespace _Game.Core.Gameplay.Combat
{
    /// <summary>
    /// Manager tổng thể cho battle flow - kết nối Map và Battle System
    /// </summary>
    public class BattleManager : MonoBehaviour
    {
        [SerializeField] public BattleMediator mediator;
        [SerializeField] private CardManager cardManager;
        [SerializeField] private Enemy enemy;
        [SerializeField] private PlayerController playerController;
        [SerializeField] private GameObject battlePanel;
        public UnityEvent onBattleStart;
        public UnityEvent onBattleWin;
        public UnityEvent onBattleComplete;
        public UnityEvent onBattleLose;

        public int enemyReward => enemy.data.reward;
        
        public void BattleStart(EnemyData enemyData)
        {
            // get 
            var deckData = GameInstance.Singleton.GetDeckData();
            // start UI
            battlePanel.SetActive(true);

            // start player
            mediator.StartBattle(enemyData);
            cardManager.StartBattle(deckData.Cards);
            // start enemy
            enemy.SetData(enemyData);
            
            // 
            onBattleStart?.Invoke();
            
            // start turn
            playerController.Action.Active();
        }
        public void CheckCondition(object sender)
        {
            if (playerController.Stat.health.CurrentValue <= 0)
            {
                HandleLose();
                return;
            }
            if (enemy.health.CurrentValue <=0)
            {
                HandleWin();
                return;
            }
            HandleNextTurn(sender);
        }

        private void HandleNextTurn(object sender)
        {
            if (sender is Enemy)
            {
                Debug.Log($"You start your turn");
                playerController.Action.StartTurn();  
                return;
            }
            if (sender is PlayerActionController)
            {
                Debug.Log($"You start enemy turn");
                enemy.HandleEnemyTurn();
            }
        }
        private void HandleWin()
        {
            Debug.Log($"You win");
            // win resolve
            HandleEnd();
            if (enemy.data is BossData)
            {
                onBattleComplete?.Invoke(); 
            }
            else
            {
                onBattleWin?.Invoke();
            }
        }

        private void HandleLose()
        {

            Debug.Log($"You lose");
            HandleEnd();
            onBattleLose?.Invoke();
        }

        private void HandleEnd()
        {
            battlePanel.SetActive(false);
            cardManager.Clear();
        }
        public void AttackPlayer(float damage)
        {
            mediator.Attack(playerController.Stat, damage);
        }

        public void AttackEnemy(float damage)
        {
            mediator.Attack(enemy,  damage);
        }
    }
}