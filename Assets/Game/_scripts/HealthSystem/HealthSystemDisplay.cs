using DG.Tweening;
using System;
using TMPro;
//using UniRx;
using UnityEngine;

public class HealthSystemDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;

    //private CompositeDisposable disposables = new CompositeDisposable();
    public float TweenDuraion = 0.3f;
    public void UpdateText(Stat healthData)
    {

        string targetText = $"{healthData.Current.Value}/{healthData.Max.Value} HP";
        Sequence sequence = DOTween.Sequence();

        sequence.Join(text.transform.DOPunchScale(Vector3.one, TweenDuraion)
            .SetEase(Ease.OutQuad));
        //sequence.Join(_scaleTweener);

        sequence.Join( DOTween.To(
            () => text.text,
            x => text.text = x,
            targetText,
            TweenDuraion)
            .SetEase(Ease.OutQuad));
        //sequence.Join(_textChange);
        sequence.Join(text.DOColor(Color.white, TweenDuraion/2)
            .From(Color.red)
            .SetLoops(2, LoopType.Yoyo)
            .OnComplete(() => text.color = Color.white));

    }
}
