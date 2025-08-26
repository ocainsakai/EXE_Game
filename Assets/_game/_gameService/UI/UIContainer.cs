using UnityEngine;

public class UIContainer : MonoBehaviour
{
    [SerializeField] private GameObject _uiPrefab;
    [SerializeField] private Transform _uiParent;
    public void ShowUI()
    {
        if (_uiPrefab != null && _uiParent != null)
        {
            GameObject uiInstance = Instantiate(_uiPrefab, _uiParent);
            uiInstance.SetActive(true);
        }
        else
        {
            Debug.LogWarning("UI Prefab or Parent is not set.");
        }
    }
    public void HideUI()
    {
        if (_uiParent != null)
        {
            foreach (Transform child in _uiParent)
            {
                child.gameObject.SetActive(false);
            }
        }
    }
}