using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace _Game.Core.Gameplay.Combat
{
    /// <summary>
    /// Manager tổng thể cho battle flow - kết nối Map và Battle System
    /// </summary>
    public class BattleManager : MonoBehaviour
    {
        [SerializeField] private BattleSystem battleSystem;
        [SerializeField] private Enemy enemy;
        [SerializeField] private PlayerActionController playerActionController;
        [SerializeField] private PlayerStatComponent playerStatComponent;
        [SerializeField] private GameObject battlePanel;
        [FormerlySerializedAs("OnBattleStart")] public UnityEvent onBattleStart;
        [FormerlySerializedAs("OnBattleEnd")] public UnityEvent onBattleEnd;
        [FormerlySerializedAs("OnBattleWin")] public UnityEvent onBattleWin;
        [FormerlySerializedAs("OnBattleLose")] public UnityEvent onBattleLose;
        public void BattleStart(EnemyData enemyData)
        {
            // start UI
            battlePanel.SetActive(true);

            // start player
            var playerData = GameInstance.Singleton.playerData;
            battleSystem.StartBattle(playerData, enemyData);
            playerActionController.gameObject.SetActive(true);
            playerStatComponent.SetData(playerData);

            // start enemy
            enemy.SetData(enemyData);
            onBattleStart?.Invoke();
        }
        public void CheckCondition(object sender)
        {
            if (playerStatComponent.Hp <= 0)
            {
                Debug.Log($"You lose");
                // lose resolve
                onBattleEnd?.Invoke();
                onBattleLose?.Invoke();
                return;
            }
            if (enemy.hp <=0)
            {
                Debug.Log($"You win");
                // win resolve
                onBattleEnd?.Invoke();
                onBattleWin?.Invoke();
                return;
            }
            if (sender != null && (sender is Enemy))
            {
                Debug.Log($"You start your turn");
                playerActionController.PlayerStartTurn();
                return;
            }
            if (sender != null && sender is PlayerActionController)
            {
                Debug.Log($"You start enemy turn");
                enemy.CountToAction();
                return;
            }
        }
        public void AttackPlayer(Enemy enemy)
        {
            playerStatComponent.Hp -= enemy.data.atk;
        }
    }
}