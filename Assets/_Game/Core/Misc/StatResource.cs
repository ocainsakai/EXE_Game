using System;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Base class quản lý tài nguyên dạng có giá trị hiện tại & tối đa (HP, Energy, Mana...)
/// Có sẵn các sự kiện để UI hoặc VFX có thể lắng nghe.
/// </summary>
[Serializable]
public class StatResource
{
    [SerializeField] protected int currentValue;
    [SerializeField] protected int maxValue;

    public int CurrentValue => currentValue;
    public int MaxValue => maxValue;

    public float Percent => maxValue > 0 ? (float)currentValue / maxValue : 0f;

    public UnityEvent<int, int> onValueChanged = new();   // (current, max)
    public UnityEvent onDepleted = new();                // Khi = 0
    public UnityEvent onFull = new();                    // Khi = max

    public StatResource(int current, int max)
    {
        maxValue = Mathf.Max(1, max);
        currentValue = Mathf.Clamp(current, 0, maxValue);
    }

    public void SetValue(int value)
    {
        int oldValue = currentValue;
        currentValue = Mathf.Clamp(value, 0, maxValue);
        TriggerEvents(oldValue);
    }

    public void SetMaxValue(int newMax, bool keepPercent = true)
    {
        float percent = Percent;
        maxValue = Mathf.Max(1, newMax);
        if (keepPercent)
            currentValue = Mathf.RoundToInt(maxValue * percent);
        else
            currentValue = Mathf.Clamp(currentValue, 0, maxValue);

        onValueChanged?.Invoke(currentValue, maxValue);
    }

    public void Add(int amount)
    {
        if (amount == 0) return;
        int oldValue = currentValue;
        currentValue = Mathf.Clamp(currentValue + amount, 0, maxValue);
        TriggerEvents(oldValue);
    }

    public void Subtract(int amount)
    {
        if (amount == 0) return;
        int oldValue = currentValue;
        currentValue = Mathf.Clamp(currentValue - amount, 0, maxValue);
        TriggerEvents(oldValue);
    }

    private void TriggerEvents(int oldValue)
    {
        if (oldValue != currentValue)
        {
            onValueChanged?.Invoke(currentValue, maxValue);
        }

        if (currentValue <= 0)
        {
            onDepleted?.Invoke();
        }
        else if (currentValue >= maxValue)
        {
            onFull?.Invoke();
        }
    }

    public void ResetToFull()
    {
        SetValue(maxValue);
    }

    public override string ToString() => $"{currentValue}/{maxValue}";
}

[Serializable]
public class Count : StatResource
{
    public Count(int current, int max) : base(current, max) { }
    public bool isFull => currentValue == maxValue;
    public void CountUp(int amount = 1) => Add(amount); 
    public void CountDown(int amount = 1) => Subtract(amount);
}
[Serializable]
public class Health : StatResource
{
    public Health(int current, int max) : base(current, max) { }

    public void Damage(int dmg) => Subtract(dmg);
    public void Heal(int amount) => Add(amount);
}

[Serializable]
public class Energy : StatResource
{
    public Energy(int current, int max) : base(current, max) { }

    public bool TryConsume(int amount)
    {
        if (currentValue < amount)
            return false;

        Subtract(amount);
        return true;
    }
}
