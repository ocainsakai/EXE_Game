using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class TypeEntry
{
    [Tooltip("AssemblyQualifiedName of the Type")]
    [SerializeField] public string typeName;
    [SerializeField] public List<ScriptableObject> items = new();

    // runtime-cached resolved Type
    [NonSerialized] private Type _cachedType;
    public Type ResolvedType
    {
        get
        {
            if (_cachedType != null) return _cachedType;
            if (string.IsNullOrEmpty(typeName)) return null;
            _cachedType = ResolveType(typeName);
            return _cachedType;
        }
    }

    public string DisplayName => ResolvedType != null ? ResolvedType.FullName : typeName ?? "None";

    private Type ResolveType(string n)
    {
        var t = Type.GetType(n);
        if (t != null) return t;
        foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
        {
            try
            {
                t = asm.GetType(n);
                if (t != null) return t;
            }
            catch { }
        }
        return null;
    }
}
