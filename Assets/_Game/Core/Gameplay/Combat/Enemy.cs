using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    public EnemyData Data;
    public float MaxHP;
    public float HP;

    public int MaxActionCount;
    public int CurrentActionCount;

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
        CurrentActionCount = data.Count;
        OnTakeDame?.Invoke(this);
    }

    public void TakeDame(float dame)
    {
        HP -= dame;
        OnTakeDame?.Invoke(this);
    }

    public void CountToAction()
    {
        Debug.Log($"Counting...");
        CurrentActionCount++;
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
