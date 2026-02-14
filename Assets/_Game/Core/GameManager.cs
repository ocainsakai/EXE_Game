using UnityEngine;

namespace _Game.Core
{
    public class GameManager : MonoBehaviour
    {

        public static GameManager instance;

        public void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
        }

        public void Start()
        {
            InitGame();
        }

        private void InitGame()
        {
        }
    }
}
