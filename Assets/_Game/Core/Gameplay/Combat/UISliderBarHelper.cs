using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using DG.Tweening;

public class UISliderBarHelper : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] TextMeshProUGUI valueText;

    private Image fillImage;
    private Color originalColor;

    private void Awake()
    {
        if (slider == null)
        {
            slider = GetComponent<Slider>();
        }

        if (valueText == null)
        {
            valueText = GetComponentInChildren<TextMeshProUGUI>();
        }

        if (slider != null)
        {
            slider.value = slider.maxValue = 1;

            // Lấy fill image từ slider
            fillImage = slider.fillRect?.GetComponent<Image>();
            Debug.Log(fillImage);
            if (fillImage != null)
            {
                originalColor = fillImage.color;
            }
        }

        if (valueText != null)
        {
            valueText.text = $"NA/NA";
        }
    }

    public void SetValue(float current, float max)
    {
        if (slider != null)
        {
            slider.maxValue = max;
            slider.value = current;
        }

        if (valueText != null)
        {
            valueText.text = $"{current}/{max}";
        }
    }

    public void SetValue(int current, int max)
    {
        FlashVFX(Color.clear, 0.5f);
        SetValue((float)current, (float)max);
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void FlashVFX(Color? flashColor = null, float duration = 0.2f)
    {
        if (!fillImage)
        {
            Debug.LogWarning("Fill image is null! Cannot flash VFX.");
            return;
        }

        Color targetColor = flashColor ?? Color.black;
        float halfDuration = duration / 2f;

        fillImage.DOColor(targetColor, halfDuration).SetEase(Ease.InFlash)
            .OnComplete(() => fillImage.DOColor(originalColor, halfDuration));
    }
}