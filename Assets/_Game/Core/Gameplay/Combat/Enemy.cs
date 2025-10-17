using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Enemy : MonoBehaviour
{
    [FormerlySerializedAs("Data")] [SerializeField]
    public EnemyData data;
    [FormerlySerializedAs("MaxHP")] public float maxHp;
    [FormerlySerializedAs("HP")] public float hp;

    [FormerlySerializedAs("MaxActionCount")] public int maxActionCount;
    private int _currentActionCount;
    public int CurrentActionCount
    {
        get => _currentActionCount;
        set
        {
            _currentActionCount = value;
            onCounting?.Invoke(_currentActionCount, maxActionCount);
        }
    }

    [FormerlySerializedAs("OnCounting")] public UnityEvent<int, int> onCounting;
    [FormerlySerializedAs("OnHealthChange")] public UnityEvent<float, float> onHealthChange;
    [FormerlySerializedAs("OnTakeDame")] public UnityEvent<Enemy> onTakeDame;
    [FormerlySerializedAs("OnDeath")] public UnityEvent<Enemy> onDeath;
    [FormerlySerializedAs("OnAction")] public UnityEvent<Enemy> onAction;
    [FormerlySerializedAs("OnEndTurn")] public UnityEvent<Enemy> onEndTurn;
    private void OnEnable()
    {
        // test
        maxHp = data.hp;
        hp = data.hp;
        maxActionCount = data.count;
        CurrentActionCount = data.count;
        onTakeDame?.Invoke(this);
    }

    public void SetData(EnemyData data) {
        this.data = data;
        maxHp = data.hp;
        hp = data.hp;
        maxActionCount = data.count;
        CurrentActionCount = 0;
        onCounting?.Invoke(CurrentActionCount, maxActionCount);
        onHealthChange?.Invoke(hp, maxHp);
    }

    public void TakeDame(float dame)
    {
        hp -= dame;
        onHealthChange?.Invoke(hp, maxHp);
        onTakeDame?.Invoke(this);
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
        if (CurrentActionCount >= maxActionCount)
        {
            onAction?.Invoke(this);
            CurrentActionCount = 0;
        }
        EndEnemyTurn();
    }
    public void EndEnemyTurn()
    {
        onEndTurn?.Invoke(this);
    }
}
