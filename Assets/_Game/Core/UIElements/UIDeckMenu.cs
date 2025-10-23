using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace _Game.Core
{
    public class UIDeckMenu : MonoBehaviour
    {
        [Header("Menu Sections")]
        [Tooltip("Panel that displays the list of characters.")]
        public GameObject deckSelection;
        [Tooltip("Panel that displays the details of a specific character.")]
        public GameObject deckDetails;

        public UnityEvent onOpenMenu;

        public UnityEvent onCloseMenu;

    
        void OnEnable()
        {
            onOpenMenu?.Invoke();
            if (deckSelection != null) deckSelection.SetActive(true);
            if (deckDetails != null) deckDetails.SetActive(false);

        }
   
        private void OnDisable()
        {
            onCloseMenu?.Invoke();
        }
    }
}
