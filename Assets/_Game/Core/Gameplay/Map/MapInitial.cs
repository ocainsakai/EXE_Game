using System;
using UnityEngine;

public class MapInitial : MonoBehaviour
{
    [SerializeField] private GameObject uiMapDetails;
    [SerializeField] private GameObject battlePanel;
    [SerializeField] private GameObject uiScreens;
    [SerializeField] private GameObject uiHome;
    [SerializeField] private GameObject uiInstruction;

    private void Start()
    {
        CloseAll();
        uiMapDetails.SetActive(true);
        uiScreens.SetActive(true);
    }

    void CloseAll()
    {
        uiMapDetails.SetActive(false);
        battlePanel.SetActive(false);
        uiScreens.SetActive(false);
        uiHome.SetActive(false);
        uiInstruction.SetActive(false);
        
    }
}
