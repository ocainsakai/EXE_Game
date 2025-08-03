using Ain;
using TMPro;
using UnityEngine;
using UniRx;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textHP;
    [SerializeField] private HealthComponent healthComponent;

    private void Awake()
    {
        if (textHP == null)
        {
            textHP = GetComponent<TextMeshProUGUI>();
        }

        healthComponent.CurrentHP.Subscribe(UpdateHP).AddTo(this);
        healthComponent.MaxHP.Subscribe(UpdateHP).AddTo(this);
        healthComponent.Shield.Subscribe(UpdateHP).AddTo(this);
    }
    private void UpdateHP(float obj)
    {
        textHP.text = $"{healthComponent.CurrentHP.Value}/{healthComponent.MaxHP.Value} HP";
        if (healthComponent.Shield.Value > 0)
        {
            textHP.text += $" (+{healthComponent.Shield.Value} Shield)";
        }
        else
        {
            textHP.text += " (No Shield)";
        }
    }
}
