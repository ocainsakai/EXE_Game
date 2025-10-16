using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    public EnemyData Data;
    public float MaxHP;
    public float HP;

    public int MaxActionCount;
    private int _currentActionCount;
    public int CurrentActionCount
    {
        get => _currentActionCount;
        set
        {
            _currentActionCount = value;
            OnCounting?.Invoke(_currentActionCount, MaxActionCount);
        }
    }

    public UnityEvent<int, int> OnCounting;
    public UnityEvent<float, float> OnHealthChange;
    public UnityEvent<Enemy> OnTakeDame;
    public UnityEvent<Enemy> OnDeath;
    public UnityEvent<Enemy> OnAction;
    public UnityEvent<Enemy> OnEndTurn;
    private void OnEnable()
    {
        // test
        MaxHP = Data.HP;
        HP = Data.HP;
        MaxActionCount = Data.Count;
        CurrentActionCount = Data.Count;
        OnTakeDame?.Invoke(this);
    }

    public void SetData(EnemyData data) {
        Data = data;
        MaxHP = data.HP;
        HP = data.HP;
        MaxActionCount = data.Count;
        CurrentActionCount = 0;
        OnCounting?.Invoke(CurrentActionCount, MaxActionCount);
        OnHealthChange?.Invoke(HP, MaxHP);
    }

    public void TakeDame(float dame)
    {
        HP -= dame;
        OnHealthChange?.Invoke(HP, MaxHP);
        OnTakeDame?.Invoke(this);
    }

    public void CountToAction()
    {
        StartCoroutine(CountRoutine());
    }
    IEnumerator CountRoutine()
    {
        Debug.Log($"Counting...");
        CurrentActionCount++;
        yield return new WaitForSeconds(1);
        if (CurrentActionCount >= MaxActionCount)
        {
            OnAction?.Invoke(this);
            CurrentActionCount = 0;
        }
        EndEnemyTurn();
    }
    public void EndEnemyTurn()
    {
        OnEndTurn?.Invoke(this);
    }
}
