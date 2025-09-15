using System;
using UnityEngine;

namespace Map
{
    public class MapUI : MonoBehaviour
    {
        public static bool IsBlocking { get; private set; } = false;

        [Header("UI Elements")]
        public GameObject HUD;
        public MapPopup PopupUI;

        private void Awake()
        {
            // Lấy popup kể cả khi bị disable
            if (PopupUI == null)
                PopupUI = GetComponentInChildren<MapPopup>(true);
        }

        private void Start()
        {
            CloseAll();
        }

        public void CloseAll()
        {
            IsBlocking = false;

            if (PopupUI != null && PopupUI.gameObject != null)
            {
                PopupUI.gameObject.SetActive(false);
            }
        }

        public void ShowPopup(object message, Action action = null)
        {
            if (PopupUI == null)
            {
                Debug.LogError("PopupUI chưa được gán hoặc đã bị destroy!");
                return;
            }

            CloseAll();

            PopupUI.gameObject.SetActive(true);
            PopupUI.Show(message, ui => IsBlocking = false, action);

            IsBlocking = true;
        }

        public void ShowMessage(object message)
        {
            // Dùng lại ShowPopup nhưng không truyền action
            ShowPopup(message, null);
        }
    }
}
