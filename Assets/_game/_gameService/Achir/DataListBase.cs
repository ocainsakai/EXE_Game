using System.Collections.Generic;
using UnityEngine;

public abstract class DataListBase  : ScriptableObject
{
    static readonly List<DataListBase> Database = new List<DataListBase>();
    protected virtual void OnEnable() { Database.Add(this); }
    protected virtual void OnDisable() { Database.Remove(this); }
}
