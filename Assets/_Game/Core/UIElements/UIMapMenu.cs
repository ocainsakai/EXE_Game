using UnityEngine;
using UnityEngine.Events; 
using UnityEngine.Serialization;

    public class UIMapMenu : MonoBehaviour
    {
        [Header("Menu Sections")]
        [Tooltip("Panel hiển thị danh sách các bản đồ.")]
        public GameObject mapSelection;
        [Tooltip("Panel hiển thị chi tiết của một bản đồ cụ thể.")]
        public GameObject mapDetails;

        public UnityEvent onOpenMenu;
        public UnityEvent onCloseMenu;

        void OnEnable()
        {
            onOpenMenu?.Invoke(); 

            if (mapSelection != null) mapSelection.SetActive(true);
            if (mapDetails != null) mapDetails.SetActive(false);
        }

        private void OnDisable()
        {
            onCloseMenu?.Invoke(); 
        }
    }
