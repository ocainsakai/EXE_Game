
using System;
using UniRx;
using UnityEngine;
//[RequireComponent(typeof(HealthSystemDisplay))]
public class HealthSystem : MonoBehaviour, IDisposable
{
    [SerializeField] private HealthSystemDisplay _display;

    private CompositeDisposable _disposable;
    public Stat Health;
    public int AnimationDuration => (int)(_display.TweenDuraion * 1000);

    public void Awake()
    {
        _disposable = new CompositeDisposable();
    }
    public void SetStat(Stat health)
    {
        this.Health = health;
        _display.UpdateText(Health);
        Health.Current.Pairwise().Subscribe(hp =>
        {
            _display.UpdateText(Health);

        });
    }

    public void TakeDame(int amount)
    {
        Health.Decrease(amount);
    }
    public void SetMaxHealth(int MaxHealth)
    {
        Health.SetMax(MaxHealth);
    }
 
    public void Dispose()
    {
        _disposable.Dispose();
    }
    private void OnDestroy()
    {
        Dispose();
    }
}
