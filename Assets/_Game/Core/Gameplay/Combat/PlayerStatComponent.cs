using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class PlayerStatComponent : MonoBehaviour
{
    [FormerlySerializedAs("MaxHp")] public float maxHp;

    private float _hp;
    public float Hp
    {
        get => _hp;
        set
        {
            if (_hp != value)
            {
                _hp = Mathf.Clamp(value, 0, maxHp);
                onHpChange?.Invoke(_hp, maxHp);
            }
        }
    }

    public void SetData(PlayerData playerData)
    {
        maxHp = playerData.hp;
        Hp = maxHp;
    }

    [FormerlySerializedAs("OnHPChange")] public UnityEvent<float, float> onHpChange; 
}
