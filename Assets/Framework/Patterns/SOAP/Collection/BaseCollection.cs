using System;
using System.Collections;

public abstract class BaseCollection : SOABaseObject, IEnumerable
{
    public object this[int index]
    {
        get
        {
            return List[index];
        }
        set
        {
            List[index] = value;
        }
    }


    public int Count { get { return List.Count; } }

    public abstract IList List { get; }
    public abstract Type Type { get; }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return List.GetEnumerator();
    }
    public bool Contains(object obj)
    {
        return List.Contains(obj);
    }


}