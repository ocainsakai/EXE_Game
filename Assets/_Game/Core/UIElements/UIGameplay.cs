using UnityEngine;

public class UIGameplay : MonoBehaviour
{
    [SerializeField] GameObject uiMapOpen;
    [SerializeField] GameObject uiDeckOpen;
    [SerializeField] GameObject uiPlayerAction;
    [SerializeField] GameObject uiCardManager;
    [SerializeField] GameObject uiMap;
    [SerializeField] GameObject uiBattle;

    void Start()
    {
        ShowOpener();
    }
    public void HideAll()
    {
        uiMapOpen.SetActive(false);
        uiDeckOpen.SetActive(false);
        uiPlayerAction.SetActive(false);
        uiCardManager.SetActive(false);
        uiMap.SetActive(false);
        uiBattle.SetActive(false);
    }
    public void ShowOpener()
    {
        HideAll();
        uiDeckOpen.SetActive(true);
        uiMapOpen.SetActive(true);
    }
    public void ShowMapOpen()
    {
        HideAll();
        uiMapOpen.SetActive(true);
    }
    public void ShowDeckOpen()
    {
        HideAll();
        uiDeckOpen.SetActive(true);
    }
    public void ShowPlayerAction()
    {
        HideAll();
        uiPlayerAction.SetActive(true);
    }
    public void ShowCardManager()
    {
        HideAll();
        uiCardManager.SetActive(true);
    }
    public void ShowMap()
    {
        HideAll();
        uiMap.SetActive(true);
    }
    public void ShowBattle()
    {
        HideAll();
        uiBattle.SetActive(true);
    }
   
}
