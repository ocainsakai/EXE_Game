using Game.Service;
using UnityEngine;

namespace Game
{
    public class BattleController : MonoBehaviour
    {
        public PlayerController playerController;
        public EnemyManager enemyManager;
        
        StateMachine stateMachine;
        private void Awake()
        {
            stateMachine = new StateMachine();
        }
        private void Start()
        {
            stateMachine.SetState(new BattleStart(this, GameManager.Instance.enemies, GameManager.Instance.playerConfig));   
        }

        private void Update()
        {
            stateMachine.Update();
            if (Input.GetKey(KeyCode.Escape))
            {
                stateMachine.ChangeState(new StopState(this));
            }
        }

        public void StartBattle()
        {

            stateMachine.ChangeState(new BattleStart(this, GameManager.Instance.enemies, GameManager.Instance.playerConfig));
        }
    }
}