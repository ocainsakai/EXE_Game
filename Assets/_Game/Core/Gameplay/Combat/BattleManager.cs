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
        [SerializeField] private BattleSystem battleSystem;
        [SerializeField] private CardManager cardManager;
        [SerializeField] private Enemy enemy;
        [SerializeField] private PlayerActionController playerActionController;
        [SerializeField] private PlayerStatComponent playerStatComponent;
        [SerializeField] private GameObject battlePanel;
        public UnityEvent onBattleStart;
        public UnityEvent onBattleWin;
        public UnityEvent onBattleLose;
        public void BattleStart(EnemyData enemyData)
        {
            // get data
            var playerData = GameInstance.Singleton.playerData;
            var deckData = GameInstance.Singleton.GetDeckData();
            // start UI
            battlePanel.SetActive(true);

            // start player
            battleSystem.StartBattle(playerData, enemyData);
            playerStatComponent.SetData(playerData);
            cardManager.StartBattle(deckData.Cards);
            // start enemy
            enemy.SetData(enemyData);
            onBattleStart?.Invoke();
            
            // start turn
            playerActionController.Active();
        }
        public void CheckCondition(object sender)
        {
            if (playerStatComponent.Hp <= 0)
            {
                HandleLose();
                return;
            }
            if (enemy.hp <=0)
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
                playerActionController.StartTurn();  
                return;
            }
            if (sender is PlayerActionController)
            {
                Debug.Log($"You start enemy turn");
                enemy.CountToAction();
            }
        }
        private void HandleWin()
        {
            Debug.Log($"You win");
            // win resolve
            onBattleWin?.Invoke();
        }

        private void HandleLose()
        {
            Debug.Log($"You lose");
            // lose resolve
            onBattleLose?.Invoke();
            
        }
        public void AttackPlayer(Enemy enemyAttacker)
        {
            playerStatComponent.Hp -= enemyAttacker.data.atk;
        }
    }
}