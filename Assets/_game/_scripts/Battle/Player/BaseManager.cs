using UnityEngine;

public abstract class BaseManager : MonoBehaviour, IManager
{
    public virtual void Init()
    {
        
    }
    public virtual void Hide()
    {
        transform.root.gameObject.SetActive(false);
    }
    public virtual void Show()
    {
        transform.root.gameObject.SetActive(true);
    }
}   