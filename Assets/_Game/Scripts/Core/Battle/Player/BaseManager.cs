using UnityEngine;
using UnityUtils;

public abstract class BaseManager<T> : Singleton<T>, IManager where T : BaseManager<T>
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
