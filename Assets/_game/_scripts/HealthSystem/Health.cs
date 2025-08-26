using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    private int currentHealth;
    private int maxHealth = 100;
    public int MaxHealth => maxHealth;

    public Action<int> onHealthChanged;
    public Action onDeath;

    public int CurrentHealth
    {
        get => currentHealth;
        set
        {
            int newHealth = Mathf.Clamp(value, 0, maxHealth);
            if (value <= 0)
            {
                onDeath?.Invoke();
            }
            if (newHealth != currentHealth)
            {
                currentHealth = newHealth;
                onHealthChanged?.Invoke(currentHealth);
            }
        }
    }

    public void Init(int maxHealth, int currentHealth)
    {
        this.maxHealth = maxHealth;
        this.currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        onHealthChanged?.Invoke(this.currentHealth);
    }

    public void Heal(int heal)
    {
        CurrentHealth += heal;
    }
    public void TakeDamege(int damege)
    {
        CurrentHealth -= damege;
    }
}
