using UnityEngine;

public class UIMainScreen : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject mapMenu;
    public GameObject shopPanel;

    public GameObject deckPanel;

    public GameObject settingPanel;

    public GameObject collectionPanel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CloseAll();
        mainMenu.SetActive(true);
    }

    void CloseAll()
    {
        mainMenu?.SetActive(false);
        mapMenu?.SetActive(false);
        shopPanel?.SetActive(false);
        deckPanel?.SetActive(false);
        settingPanel?.SetActive(false);
        collectionPanel?.SetActive(false);
        
    }
}
