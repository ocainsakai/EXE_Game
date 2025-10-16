using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISliderBarHelper : MonoBehaviour
{
    [SerializeField] Slider _slider;
    [SerializeField] TextMeshProUGUI _valueText;
    public void Awake()
    {
        if (_slider == null)
        {
            _slider = GetComponent<Slider>();
        }
        if( _valueText == null)
        {
            _valueText = GetComponentInChildren<TextMeshProUGUI>();
        }
        if (_slider != null)
        {
            _slider.value = _slider.maxValue = 1;
        }
        if (_valueText != null)
        {
            _valueText.text = $"NA/NA";
        }
    }

    public void SetValue(float current, float max)
    {
        if (_slider != null)
        {
            _slider.maxValue = max;
            _slider.value = current;
        }
        if (_slider != null && _valueText != null)
        {
            _valueText.text = $"{current}/{max}";
        }
    }
    public void SetValue(int current, int max)
    {
        if (_slider != null)
        {
            _slider.maxValue = max;
            _slider.value = current;
        }
        if (_slider != null && _valueText != null)
        {
            _valueText.text = $"{current}/{max}";
        }
    }
}

