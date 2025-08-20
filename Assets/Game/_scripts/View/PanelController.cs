using UnityEngine;

public class PanelController : MonoBehaviour
{
    public GameObject HUD;
    public GameObject ChoosingUI;
    
    public void CloseAll()
    {
        HUD.SetActive(false);
        ChoosingUI.SetActive(false);
    }
}
