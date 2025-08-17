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
    [SerializeField] private float textUpdateDuration = 0.5f;
    [SerializeField] private float punchScaleAmount = 0.2f;

    
    private Tweener _textTweener;
    private Tweener _scaleTweener;
    private CancellationTokenSource _cts;

    public int CountTime => (int) ((textUpdateDuration + punchScaleAmount) * 1000);
    private void Awake()
    {
        _cts = new CancellationTokenSource();
    }
    string GetProgressBar(int current, int max, string completed = "■", string remaining = "□")
    {
        return new string(completed[0], current) + new string(remaining[0], max - current);
    }
    public async UniTask UpdateCounterTextAsync(int currentCount, int maxCount)
    {
       
        // Kill any existing tweens
        _textTweener?.Kill();
        _scaleTweener?.Kill();

        try
        {
            // Create the new text value
            string targetText = $"{GetProgressBar(currentCount, maxCount)} ({currentCount}/{maxCount})"; 

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
            if (currentCount < maxCount)
            {
                sequence.Join(counterText.DOColor(Color.red, textUpdateDuration / 4)
                    .SetLoops(2, LoopType.Yoyo))
                    .OnComplete(() => counterText.color = Color.white);
            }

            await sequence.AsyncWaitForCompletion();
        }
        catch (OperationCanceledException)
        {
            // Handle cancellation
        }
    }


    private void OnDestroy()
    {
        _cts?.Cancel();
        _cts?.Dispose();
        _textTweener?.Kill();
        _scaleTweener?.Kill();
    }
}