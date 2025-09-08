using System;
using UnityEngine;
using UnityEngine.UI;

public class MapPopup : UIPopup
{
    [SerializeField] Button action;
    [SerializeField] Button close;

    private void Awake()
    {
        close.onClick.RemoveAllListeners();
        close.onClick.AddListener(Hide);
    }
    public void Show(object data = null, Action callback = null, Action<UIBase> onClose = null)
    {
        if (action != null)
        {
            action.onClick.RemoveAllListeners();

            if (callback != null)
            {
                action.onClick.AddListener(() => {
                    callback.Invoke();
                    Hide();
                });
                action.gameObject.SetActive(true);
            }
            else
            {
                action.gameObject.SetActive(false);
            }
        }
        base.Show(data, onClose);
        
    }

}
