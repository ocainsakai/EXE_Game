using System;
using UniRx;
using UnityEngine;

[System.Serializable]
public class Stat
{
    [SerializeField]
    private ReactiveProperty<float> current;
    public IReactiveProperty<float> Current => current;
    [SerializeField]
    private ReactiveProperty<float> max;
    public IReactiveProperty<float> Max => max;

    public Stat(float maxValue)
    {
        max = new ReactiveProperty<float>(maxValue);
        current = new ReactiveProperty<float>(maxValue);
    }
    public bool IncreaseMax(int amount)
    {
        if (amount < 0) return false;
        max.Value += amount;
        current.Value += amount;
        return true;
    }
    public bool DecreaseMax(int amount)
    {
        if (amount < 0) return false;
        max.Value -= Mathf.Max(0, max.Value - amount);
        current.Value = Mathf.Min(current.Value, max.Value);
        return true;
    }
    public bool SetMax(int newMax)
    {
        if (newMax < 0) return false;
        max.Value = newMax;
        current.Value = Mathf.Min(current.Value, max.Value);
        return true;
    }
    public bool Increase(int amount)
    {
        if (amount < 0) return false;
        current.Value = Mathf.Min(current.Value + amount, max.Value);
        return true;
    }
    public bool Decrease(int amount)
    {
        if (amount < 0 || amount > current.Value) return false;
        current.Value -= amount;
        return true;
    }
    public bool SetCurrent(int newCurrent)
    {
        if (newCurrent < 0) return false;
        //max.Value = newMax;
        current.Value = Mathf.Min(newCurrent, max.Value);
        return true;
    }
}
