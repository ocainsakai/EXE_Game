using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class PlayerStatComponent : MonoBehaviour, IHealth
{
    [SerializeField] private UISliderBarHelper healthBarHelper;
    public Health health;

    public void SetData(PlayerData playerData)
    {
        health = new((int)playerData.hp,(int) playerData.hp);
        health.onValueChanged.AddListener(healthBarHelper.SetValue);
    }

    [ContextMenu("Test HP")]
    private void TestTakeDame()
    {
        TakeDame(10f);
    }
    
    public void TakeDame(float damage)
    {
        health.Subtract((int)damage);   
    }
}

public interface IHealth
{
    public void TakeDame(float damage);
}
