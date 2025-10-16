using System;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStatComponent : MonoBehaviour
{
    public float MaxHp;

    private float _hp;
    public float HP
    {
        get => _hp;
        set
        {
            if (_hp != value)
            {
                _hp = Mathf.Clamp(value, 0, MaxHp);
                OnHPChange?.Invoke(_hp, MaxHp);
            }
        }
    }

    public void SetData(PlayerData playerData)
    {
        MaxHp = playerData.HP;
        HP = MaxHp;
    }

    public UnityEvent<float, float> OnHPChange; 
}
