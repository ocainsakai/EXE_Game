using UnityEngine;

namespace _Game.Core.Flow
{
    public abstract class GamePanel : MonoBehaviour
    {
        public abstract GameFlowState State { get; }
        
        public virtual void OnEnter(object data = null)
        {
            gameObject.SetActive(true);
        }
        
        public virtual void OnExit()
        {
            gameObject.SetActive(false);
        }
    }

    public enum GameFlowState
    {
        MainMenu,
        Shop,
        Settings
    }
}
