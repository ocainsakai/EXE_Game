using TMPro;
using UnityEngine;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] Health health;
    public TextMeshProUGUI healthText;

    private void OnEnable()
    {
        health.onHealthChanged += UpdateHealthDisplay;
        UpdateHealthDisplay(health.CurrentHealth);
    }
    private void OnDisable()
    {
        health.onHealthChanged -= UpdateHealthDisplay;
    }
    private void UpdateHealthDisplay(int currentHealth)
    {
        healthText.text = $"{currentHealth} / {health.MaxHealth}";
    }
}
