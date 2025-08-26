using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DataList<T> : DataListBase, IList<T> where T : DataAsset
{
    [SerializeField] protected List<T> _list = new List<T>();

    // maintain counts / fast lookup if duplicates allowed (e.g. inventory-like lists)
    private Dictionary<T, int> _itemCounts = new Dictionary<T, int>();

    // optional event when list changes (runtime & editor)
    public event Action OnChanged;

    #region IList<T> properties
    public int Count => _list.Count;
    public bool IsReadOnly => false;
    public bool IsEmpty => _list.Count == 0;
    public Type GetGenericType => typeof(T);

    public T this[int index]
    {
        get => _list[index];
        set
        {
            if (index < 0 || index >= _list.Count) throw new ArgumentOutOfRangeException(nameof(index));
            var old = _list[index];
            if (EqualityComparer<T>.Default.Equals(old, value)) return;
            // update counts
            DecrementCount(old);
            _list[index] = value;
            IncrementCount(value);
            NotifyChanged();
        }
    }
    #endregion

    #region Core IList<T> methods
    public void Add(T item)
    {
        if (item == null) throw new ArgumentNullException(nameof(item));
        _list.Add(item);
        IncrementCount(item);
        NotifyChanged();
    }

    public void Clear()
    {
        _list.Clear();
        _itemCounts.Clear();
        NotifyChanged();
    }

    public bool Contains(T item) => _itemCounts.ContainsKey(item) && _itemCounts[item] > 0;

    public void CopyTo(T[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);

    public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();

    public int IndexOf(T item) => _list.IndexOf(item);

    public void Insert(int index, T item)
    {
        if (item == null) throw new ArgumentNullException(nameof(item));
        if (index < 0 || index > _list.Count) throw new ArgumentOutOfRangeException(nameof(index));
        _list.Insert(index, item);
        IncrementCount(item);
        NotifyChanged();
    }

    public bool Remove(T item)
    {
        if (item == null) return false;
        bool removed = _list.Remove(item);
        if (removed) { DecrementCount(item); NotifyChanged(); }
        return removed;
    }

    public void RemoveAt(int index)
    {
        if (index < 0 || index >= _list.Count) throw new ArgumentOutOfRangeException(nameof(index));
        var item = _list[index];
        _list.RemoveAt(index);
        DecrementCount(item);
        NotifyChanged();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    #endregion

    #region Item counts helpers
    private void IncrementCount(T item)
    {
        if (item == null) return;
        if (_itemCounts.TryGetValue(item, out var c)) _itemCounts[item] = c + 1;
        else _itemCounts[item] = 1;
    }

    private void DecrementCount(T item)
    {
        if (item == null) return;
        if (_itemCounts.TryGetValue(item, out var c))
        {
            if (c <= 1) _itemCounts.Remove(item);
            else _itemCounts[item] = c - 1;
        }
    }

    /// <summary>
    /// Trả về số lượng instance của item trong list (0 nếu không có).
    /// </summary>
    public int CountOf(T item)
    {
        if (item == null) return 0;
        return _itemCounts.TryGetValue(item, out var c) ? c : 0;
    }
    #endregion

    #region Utility / Lookup
    /// <summary> Return a shallow copy of internal list so caller can't mutate internal list directly. </summary>
    public IReadOnlyList<T> AsReadOnly() => _list.AsReadOnly();

    /// <summary>Find first element matching predicate</summary>
    public T Find(Predicate<T> predicate) => _list.Find(predicate);

    /// <summary>Find all matches</summary>
    public List<T> FindAll(Predicate<T> predicate) => _list.FindAll(predicate);

    /// <summary>Try get item by id-like field. Requires T implements IDataAsset-like Id property (optional).</summary>
    public virtual T GetById(string id)
    {
        // Best-effort: if T exposes "id" or "Id" field/property via reflection, try to match.
        if (string.IsNullOrEmpty(id)) return null;
        foreach (var it in _list)
        {
            if (it == null) continue;
            // try common patterns: Id, id, assetId, key
            var t = it.GetType();
            var p = t.GetProperty("Id") ?? t.GetProperty("id") ?? t.GetProperty("AssetId") ?? t.GetProperty("Key");
            if (p != null)
            {
                var val = p.GetValue(it)?.ToString();
                if (string.Equals(val, id, StringComparison.OrdinalIgnoreCase)) return it;
            }
            else
            {
                var f = t.GetField("id") ?? t.GetField("Id") ?? t.GetField("assetId");
                if (f != null)
                {
                    var val = f.GetValue(it)?.ToString();
                    if (string.Equals(val, id, StringComparison.OrdinalIgnoreCase)) return it;
                }
            }
        }
        return null;
    }
    #endregion

    #region Validation & OnEnable/OnValidate
    protected override void OnEnable()
    {
        base.OnEnable();
        // rebuild counts in case domain reload or serialization changed
        RebuildCounts();
#if UNITY_EDITOR
        // ensure editor sees change
        UnityEditor.EditorApplication.delayCall += () => { NotifyChanged(); };
#endif
    }

    /// <summary> Rebuild the internal counts dictionary from _list </summary>
    protected void RebuildCounts()
    {
        _itemCounts.Clear();
        foreach (var it in _list)
        {
            if (it == null) continue;
            if (_itemCounts.TryGetValue(it, out var c)) _itemCounts[it] = c + 1;
            else _itemCounts[it] = 1;
        }
    }
    #endregion

    #region Notifications / Debug
    protected void NotifyChanged()
    {
        OnChanged?.Invoke();
#if UNITY_EDITOR
        // mark dirty in editor so change persists
        if (!Application.isPlaying)
        {
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
    }

    public override string ToString()
    {
        return $"{GetType().Name} (Count={Count})";
    }
    #endregion
}
