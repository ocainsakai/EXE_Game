using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Threading;
using TMPro;
using UniRx;
using UnityEngine;

public class EnemyCounterSystem : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int _initialCount = 1;
    [SerializeField] private int _maxCount = 3;
    [SerializeField] private float _countChangeDuration = 0.5f;

    [Header("References")]
    [SerializeField] private TextMeshProUGUI _counterText;

    private ReactiveProperty<int> _currentCount;
    private ReactiveProperty<int> _maxCountValue;
    private CancellationTokenSource _cts;

    public IReadOnlyReactiveProperty<int> CurrentCount => _currentCount;
    public IReadOnlyReactiveProperty<int> MaxCount => _maxCountValue;
    public event Action OnCountReachedZero;

    private void Awake()
    {
        _cts = new CancellationTokenSource();
        _currentCount = new ReactiveProperty<int>(_initialCount);
        _maxCountValue = new ReactiveProperty<int>(_maxCount);

        //CurrentCount.Subscribe(_ => UpdateCounterDisplay().Forget());
        //MaxCount.Subscribe(_ => UpdateCounterDisplay().Forget());
    }

    public async UniTask DecreaseCount()
    {
        if (_currentCount.Value <= 0) return;

        _currentCount.Value = Mathf.Max(_currentCount.Value - 1, 0);

        if (_currentCount.Value == 0)
        {
            OnCountReachedZero?.Invoke();
        }

        await PlayCountChangeAnimation();
    }

    private async UniTask PlayCountChangeAnimation()
    {
        try
        {
            var sequence = DOTween.Sequence();

            // Text animation
            sequence.Join(DOTween.To(
                () => _counterText.text,
                x => _counterText.text = x,
                $"{_currentCount.Value}/{_maxCountValue.Value}",
                _countChangeDuration));

            // Scale animation
            sequence.Join(_counterText.transform.DOPunchScale(
                Vector3.one * 0.2f,
                _countChangeDuration / 2));

            await sequence.AsyncWaitForCompletion();
        }
        catch (OperationCanceledException)
        {
            // Handle cancellation
        }
    }

    public void ResetCounter()
    {
        _currentCount.Value = _maxCountValue.Value;
    }

    private void OnDestroy()
    {
        _cts?.Cancel();
        _cts?.Dispose();
    }
}