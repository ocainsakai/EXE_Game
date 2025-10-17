using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UISliderBarHelper : MonoBehaviour
{
    [FormerlySerializedAs("_slider")] [SerializeField] Slider slider;
    [FormerlySerializedAs("_valueText")] [SerializeField] TextMeshProUGUI valueText;
    public void Awake()
    {
        if (slider == null)
        {
            slider = GetComponent<Slider>();
        }
        if( valueText == null)
        {
            valueText = GetComponentInChildren<TextMeshProUGUI>();
        }
        if (slider != null)
        {
            slider.value = slider.maxValue = 1;
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
        if (slider != null && valueText != null)
        {
            valueText.text = $"{current}/{max}";
        }
    }
    public void SetValue(int current, int max)
    {
        if (slider != null)
        {
            slider.maxValue = max;
            slider.value = current;
        }
        if (slider != null && valueText != null)
        {
            valueText.text = $"{current}/{max}";
        }
    }
}

