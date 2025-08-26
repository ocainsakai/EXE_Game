using UnityEngine;
using UnityEngine.Events;

namespace Map
{
    public static class UIState
    {
        public static bool IsBlocking { get; set; } = false;
    }
    public class MapUI : MonoBehaviour
    {
        public UnityEvent OnCloseAll;
        public GameObject HUD;
        public GameObject ChoosingUI;

        private void Start()
        {

            CloseAll();
        }
        public void CloseAll()
        {
            UIState.IsBlocking = false;
            ChoosingUI.SetActive(false);
            OnCloseAll?.Invoke();
        }

        public void OpenPopupUI(HexRuntime runtimeData, bool isValue)
        {
            CloseAll();
            ChoosingUI.SetActive(true);
            UIState.IsBlocking = true;
            ChoosingUI.GetComponent<PopupUI>().UpdateUI(runtimeData,isValue);

        }


    }
}

