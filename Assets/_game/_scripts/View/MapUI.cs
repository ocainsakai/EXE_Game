using UnityEngine;
using UnityEngine.Events;
using UnityServiceLocator;

namespace Map
{
    public class MapUI : MonoBehaviour
    {
        public static bool IsBlocking { get; set; } = false;
        public UnityEvent OnCloseAll;
        public GameObject HUD;
        public GameObject ChoosingUI;
        private void Awake()
        {
            ServiceLocator.ForSceneOf(this).Register(typeof(MapUI),this);
        }
        private void Start()
        {

            CloseAll();
        }

        public void CloseAll()
        {
            IsBlocking = false;
            ChoosingUI.SetActive(false);
            OnCloseAll?.Invoke();
        }

        public void OpenPopupUI(HexState runtimeData, bool isValue)
        {
            CloseAll();
            ChoosingUI.SetActive(true);
            IsBlocking = true;

        }


    }
}

