using System;
using UnityEngine;
using UnityUtils;

namespace Map
{
    public class MapUI : MonoBehaviour
    {
        public static bool IsBlocking { get; private set; } = false;

        [Header("UI Elements")]
        public GameObject HUD;
        private MapPopup _popupUI;
        //public MapPopup PopupUI;

        private void Awake()
        {
            // Lấy popup kể cả khi bị disable

            if (_popupUI.OrNull() == null)
            {
                _popupUI = GetComponentInChildren<MapPopup>(true); // true để lấy cả inactive
            }
        }
        

        private void Start()
        {
            CloseAll();
        }

        public void CloseAll()
        {
            IsBlocking = false;

            if (_popupUI != null && _popupUI.gameObject != null)
            {
                _popupUI.gameObject.SetActive(false);
            }
        }

        public void ShowPopup(object message, Action action = null)
        {
            if (_popupUI.OrNull() == null)
            {
                _popupUI = GetComponentInChildren<MapPopup>(true);
            }

            if (_popupUI == null)
            {
                Debug.LogError("PopupUI chưa được gán hoặc đã bị destroy!");
                return;
            }
            CloseAll();

            _popupUI.gameObject.SetActive(true);
            _popupUI    .Show(message, ui => IsBlocking = false, action);

            IsBlocking = true;
        }

        public void ShowMessage(object message)
        {
            // Dùng lại ShowPopup nhưng không truyền action
            ShowPopup(message, null);
        }
    }
}
