using UniRx;
using UnityEngine;

public class GameTurner : MonoBehaviour, ICountable
{
    
    protected ReactiveProperty<int> _count;
    public int Count => _count.Value;
 
    public virtual void StartTurn()
    {
        
    }
    public void DecreaseCount(int amount)
    {
        _count.Value -= amount;
    }
    public void IncreaseCount(int amount)
    {
        _count.Value += amount;
    }
}