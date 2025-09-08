using System;
using UnityEngine;
using UnityEngine.Events;

namespace Map
{
    public class MapUI : MonoBehaviour
    {
        public static bool IsBlocking { get; set; } = false;
        public GameObject HUD;
        public MapPopup PopupUI;
        private void Start()
        {

            CloseAll();
        }

        public void CloseAll()
        {
            IsBlocking = false;
            PopupUI.gameObject.SetActive(false);
        }

        public void OpenPopupUI(HexState runtimeData, Action action)
        {
            CloseAll();
            PopupUI.gameObject.SetActive(true);
            PopupUI.Show(runtimeData, action, ui => IsBlocking = false );
            IsBlocking = true;

        }


    }
}

