using UnityEngine;

public class PopupTest : MonoBehaviour
{
    [SerializeField] UIPopupManager popupManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        popupManager.ShowPopup(UIPopupName.InventoryPopup);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
