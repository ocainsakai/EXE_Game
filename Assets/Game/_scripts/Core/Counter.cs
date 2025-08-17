using System;
using UniRx;
using UnityEngine;

public class Counter : MonoBehaviour
{
    [SerializeField] CounterDisplay _display;
    [Tooltip("Current count value")]
    private ReactiveProperty<int> _currentCount;

    [Tooltip("Maximum count value")]
    private ReactiveProperty<int> _maxCount;


    public IReadOnlyReactiveProperty<int> CurrentCount => _currentCount;
    public IReadOnlyReactiveProperty<int> MaxCount => _maxCount;
    public bool IsEmpty => _currentCount.Value <= 0;
    public int CountTime => _display.CountTime;
    public void Initialize(
        int initialCount,
        int initialMaxCount,
        Action onCurrentCountChanged = null,
        Action onCountReachedZero = null,
        Action onMaxCountChanged = null)
    {
        _currentCount = new ReactiveProperty<int>(initialCount);
        _maxCount = new ReactiveProperty<int>(initialMaxCount);

        _currentCount.Subscribe(newCount =>
        {
            _ = _display.UpdateCounterTextAsync(_currentCount.Value, _maxCount.Value);
            onCurrentCountChanged?.Invoke();
            if (IsEmpty)
            {
                onCountReachedZero?.Invoke();
            }
        }).AddTo(this);

        _maxCount.Subscribe(newMaxCount =>
        {
            _ = _display.UpdateCounterTextAsync(_currentCount.Value, _maxCount.Value);
            onMaxCountChanged?.Invoke();

            ClampCurrentCount();
        }).AddTo(this);
    }

    public void IncreaseCount(int amount = 1)
    {
        if (amount <= 0)
        {
            Debug.LogWarning($"Attempted to increase with non-positive amount: {amount}");
            return;
        }

        _currentCount.Value = Mathf.Min(_currentCount.Value + amount, _maxCount.Value);
    }

    public void DecreaseCount(int amount = 1)
    {
        if (amount <= 0)
        {
            Debug.LogWarning($"Attempted to decrease with non-positive amount: {amount}");
            return;
        }

        _currentCount.Value = Mathf.Max(_currentCount.Value - amount, 0);
    }
    public void ResetCount()
    {
        _currentCount.Value = MaxCount.Value;
    }
    public void SetMaxCount(int newMaxCount, bool adjustCurrentCount = false)
    {
        if (newMaxCount <= 0)
        {
            Debug.LogError("Max count must be positive");
            return;
        }

        _maxCount.Value = newMaxCount;

        if (adjustCurrentCount)
        {
            ClampCurrentCount();
        }
    }

    private void ClampCurrentCount()
    {
        _currentCount.Value = Mathf.Min(_currentCount.Value, _maxCount.Value);
    }
}