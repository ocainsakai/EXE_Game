using UniRx;

[System.Serializable]
public class HealthData
{
    public ReactiveProperty<int> CurrentHealth ;
    public ReactiveProperty<int> MaxHealth ;
    public ReactiveProperty<int> Shield;

    public HealthData(int maxHealth, int shield = 0)
    {
        MaxHealth = new ReactiveProperty<int>(maxHealth);
        CurrentHealth = new ReactiveProperty<int>(maxHealth);
        Shield = new ReactiveProperty<int>(shield);
    }
    public void ResetHealth()
    {
        CurrentHealth.Value = MaxHealth.Value;
    }
}
