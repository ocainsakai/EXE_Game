using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Threading;
using TMPro;
using UniRx;
using UnityEngine;

public class CounterDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI counterText;
    [SerializeField] private Counter counter;
    [SerializeField] private float textUpdateDuration = 0.5f;
    [SerializeField] private float punchScaleAmount = 0.2f;

    private Tweener _textTweener;
    private Tweener _scaleTweener;
    private CancellationTokenSource _cts;

    private void Awake()
    {
        _cts = new CancellationTokenSource();
        counter = counter ?? GetComponent<Counter>();
        counterText = counterText ?? GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        if (counter != null)
        {
            counter.CurrentCount.Subscribe(_ => UpdateCounterTextAsync().Forget());
            counter.MaxCount.Subscribe(_ => UpdateCounterTextAsync().Forget());
            UpdateCounterTextAsync().Forget();
        }
    }

    public async UniTask UpdateCounterTextAsync()
    {
        if (counterText == null || counter == null)
        {
            Debug.LogWarning("Counter or CounterText is not set.");
            return;
        }

        // Kill any existing tweens
        _textTweener?.Kill();
        _scaleTweener?.Kill();

        try
        {
            // Create the new text value
            string targetText = $"{counter.CurrentCount.Value} / {counter.MaxCount.Value}";

            // Animation sequence
            var sequence = DOTween.Sequence();

            // 1. Scale punch effect
            sequence.Join(counterText.transform.DOPunchScale(
                Vector3.one * punchScaleAmount,
                textUpdateDuration / 2,
                1,
                0.5f));

            // 2. Text change animation
            sequence.Join(DOTween.To(
                () => counterText.text,
                x => counterText.text = x,
                targetText,
                textUpdateDuration));

            // 3. Color flash (optional)
            if (counter.CurrentCount.Value < counter.MaxCount.Value)
            {
                sequence.Join(counterText.DOColor(Color.red, textUpdateDuration / 4)
                    .SetLoops(2, LoopType.Yoyo));
            }

            await sequence.AsyncWaitForCompletion();
        }
        catch (OperationCanceledException)
        {
            // Handle cancellation
        }
    }

    public async UniTask PlayCountChangeAnimation()
    {
        await UpdateCounterTextAsync();
    }

    private void OnDestroy()
    {
        _cts?.Cancel();
        _cts?.Dispose();
        _textTweener?.Kill();
        _scaleTweener?.Kill();
    }
}