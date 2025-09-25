using CardSystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIDeckMenu : MonoBehaviour
{
    [Header("Menu Sections")]
    [Tooltip("Panel that displays the list of characters.")]
    public GameObject deckSelection;
    [Tooltip("Panel that displays the details of a specific character.")]
    public GameObject deckDetails;

    public UnityEvent OnOpenMenu;

    public UnityEvent OnCloseMenu;

    
    void OnEnable()
    {
        OnOpenMenu?.Invoke();
        if (deckSelection != null) deckSelection.SetActive(true);
        if (deckDetails != null) deckDetails.SetActive(false);

    }
   
    private void OnDisable()
    {
        OnCloseMenu?.Invoke();
    }
}
