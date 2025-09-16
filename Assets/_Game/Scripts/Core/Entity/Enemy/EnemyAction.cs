using System;
using UnityEngine;

public class EnemyAction : MonoBehaviour
{
    [SerializeField] TextDisplay textDisplay;
    private int _currentCount;
    private int _maxCount;
    public int MaxCount
    {
        get => _maxCount;
        set {
            if (_maxCount == value) return;
            _maxCount = Mathf.Max(value, 1);
            _maxCount = value;
            onMaxCountChanged?.Invoke();
        }
    }
    public Action onCountChanged;
    public Action onMaxCountChanged;

    public int currentCount
    {
        get => _currentCount;
        set
        {
            if (_currentCount == value) return;
            _currentCount = Mathf.Clamp(value, 0, _maxCount);
            onCountChanged?.Invoke();

            textDisplay.UpdateContent($"{_currentCount}/{_maxCount}");
        }
    }

    public void CountToAction()
    {
        currentCount--;

        if (currentCount == 0)
        {
            Debug.Log("do attack");
            currentCount = _maxCount;
        }
    }
}
