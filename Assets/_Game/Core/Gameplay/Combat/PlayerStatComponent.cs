using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class PlayerStatComponent : MonoBehaviour, IHealth
{
    [SerializeField] private UISliderBarHelper healthBarHelper;
    public Health health;
    [SerializeField]
    private PlayerData playerData;

    private void Start()
    {
        SetData(playerData);
    }

    public void SetData(PlayerData playerData)
    {
        health = new((int)playerData.hp,(int) playerData.hp);
        health.onValueChanged.AddListener(healthBarHelper.SetValue);
    }
#if UNITY_EDITOR
    [ContextMenu("Test HP")]
    private void TestTakeDame()
    {
        TakeDame(10f);
    }
#endif
    public void TakeDame(float damage)
    {
        health.Subtract((int)damage);   
    }
}

public interface IHealth
{
    public void TakeDame(float damage);
}
