using TMPro;
using UnityEngine;
using UniRx;
public class CounterDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI counterText;
    [SerializeField] Counter counter;

    private void Awake()
    {
        if (counter == null)
        {
            counter = GetComponent<Counter>();
        }
        if (counterText == null)
        {
            counterText = GetComponent<TextMeshProUGUI>();
        }
    }
    private void Start()
    {
        if (counter != null)
        {
            counter.CurrentCount.Subscribe(x => UpdateCounterText());
            counter.MaxCount.Subscribe(x => UpdateCounterText());
            UpdateCounterText();
        }
    }
    private void UpdateCounterText()
    {
        if (counterText != null && counter != null)
        {
            counterText.text = $"{counter.CurrentCount.Value} / {counter.MaxCount.Value}";
        }
        else
        {
            Debug.LogWarning("Counter or CounterText is not set.");
        }
    }   
}
