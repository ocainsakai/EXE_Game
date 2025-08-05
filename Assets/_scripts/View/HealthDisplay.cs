using Ain;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UniRx;
using UnityEngine;

public class HealthDisplay : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI textHP;
    [SerializeField] private TextMeshProUGUI textShield;
    [SerializeField] private HealthComponent healthComponent;

    [Header("Animation Settings")]
    [SerializeField] private float punchScaleDuration = 0.3f;
    [SerializeField] private float textChangeDuration = 0.5f;
    [SerializeField] private float colorFlashDuration = 0.5f;
    [SerializeField] private Vector3 punchScaleStrength = new Vector3(0.2f, 0.2f, 0f);

    private Tweener _hpTextTweener;
    private Tween _shieldTextTweener;
    private Tweener _scaleTweener;
    private Tweener _colorTweener;

    private void Start()
    {
        ValidateComponents();
        SetupObservables();
    }

    private void ValidateComponents()
    {
        if (textHP == null) textHP = GetComponent<TextMeshProUGUI>();
        if (healthComponent == null) healthComponent = GetComponentInParent<HealthComponent>();
    }

    private void SetupObservables()
    {
        healthComponent.CurrentHP
            .Pairwise()
            .Subscribe(hpPair => UpdateHP(hpPair.Previous, hpPair.Current))
            .AddTo(this);

        healthComponent.MaxHP
            .Subscribe(UpdateMaxHP)
            .AddTo(this);

        healthComponent.Shield
            .Subscribe(UpdateShield)
            .AddTo(this);
    }

    private void UpdateMaxHP(float maxHP)
    {
        UpdateHPText();
    }

    private async void UpdateShield(float shield)
    {
        _shieldTextTweener?.Kill();

        if (shield > 0)
        {
            textShield.text = $"+{shield} Shield";
            textShield.gameObject.SetActive(true);

            // Shield gain animation
            _shieldTextTweener = DOTween.Sequence()
                .Append(textShield.transform.DOScale(1.2f, 0.2f))
                .Append(textShield.transform.DOScale(1f, 0.2f))
                .Join(textShield.DOFade(1f, 0.2f).From(0f));
        }
        else
        {
            // Shield lose animation
            await textShield.DOFade(0f, 0.2f).AsyncWaitForCompletion();
            textShield.gameObject.SetActive(false);
        }
    }

    private async void UpdateHP(float oldHP, float currentHP)
    {
        KillActiveTweens();

        float hpChange = currentHP - oldHP;
        string targetText = $"{currentHP}/{healthComponent?.MaxHP.Value??0} HP";

        // Scale punch animation
        _scaleTweener = textHP?.transform.DOPunchScale(punchScaleStrength, punchScaleDuration)
            .SetEase(Ease.OutQuad);

        // Text change animation
        _hpTextTweener = DOTween.To(
            () => textHP.text,
            x => textHP.text = x,
            targetText,
            textChangeDuration)
            .SetEase(Ease.OutQuad);

        // Color flash based on HP change
        if (hpChange < 0) // Damage taken
        {
            await FlashColor(Color.red);
        }
        else if (hpChange > 0) // Healing received
        {
            await FlashColor(Color.green);
        }
    }

    private async UniTask FlashColor(Color flashColor)
    {
        _colorTweener?.Kill();

        var originalColor = textHP.color;
        _colorTweener = textHP.DOColor(flashColor, colorFlashDuration / 2)
            .OnComplete(() => textHP.DOColor(originalColor, colorFlashDuration / 2));

        await _colorTweener.AsyncWaitForCompletion();
    }

    private void UpdateHPText()
    {
        textHP.text = $"{healthComponent.CurrentHP.Value}/{healthComponent.MaxHP.Value} HP";
    }

    private void KillActiveTweens()
    {
        _hpTextTweener?.Kill();
        _scaleTweener?.Kill();
        _colorTweener?.Kill();
        _shieldTextTweener?.Kill();
    }

    private void OnDestroy()
    {
        KillActiveTweens();
    }
}