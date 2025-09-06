using UnityEngine;

namespace Game
{
    public class StopState : BattleState
    {
        public StopState(BattleController controller) : base(controller)
        {
        }

        public override void OnEnter()
        {
            Debug.Log("pause");
            Time.timeScale = 0f;
            base.OnEnter();
        }
        public override void Update()
        {
            Debug.Log("on pause");
            if (Input.GetKeyUp(KeyCode.F)) {
                controller.StartBattle();
            }
            base.Update();
        }
        public override void OnExit()
        {
            Time.timeScale = 1f;

            base.OnExit();
        }
    }
}