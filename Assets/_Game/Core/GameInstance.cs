using UnityEngine;

namespace _Game.Core
{
    [DefaultExecutionOrder(-1)]
    public class GameInstance : MonoBehaviour
    {
        // --- Singleton ---
        public static GameInstance Singleton;
        private bool isInitialized;

        private void Awake()
        {
            if (Singleton == null)
            {
                Singleton = this;
                DontDestroyOnLoad(gameObject);
                
                _Game.Core.DI.DIContainer.Global.Register<GameInstance>(this);
                
                isInitialized = true;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public bool IsInitialized() => isInitialized;
    }
}
