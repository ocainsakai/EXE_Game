using System.Collections.Generic;
using UnityEngine;

public abstract class BaseDatabase<TData> : ScriptableObject
    where TData : ScriptableObject, IData
{
    [SerializeField] protected List<TData> items = new List<TData>();
    protected Dictionary<SerializableGuid, TData> lookup;

    // call in OnEnable or manually Init()
    protected virtual void InitLookup()
    {
        if (lookup != null) return;
        lookup = new Dictionary<SerializableGuid, TData>();
        foreach (var d in items)
        {
            if (d == null) continue;
            if (lookup.ContainsKey(d.ID)) Debug.LogWarning($"Duplicate id {d.ID} in {name}");
            lookup[d.ID] = d;
        }
    }
    public virtual IEnumerable<TData> GetAllData()
    {
        return items;
    }
    public virtual TData GetData(SerializableGuid id)
    {
        if (lookup == null) InitLookup();
        lookup.TryGetValue(id, out var d);
        return d;
    }

}
