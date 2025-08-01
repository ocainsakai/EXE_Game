using UniRx;

public class GameTurner : GameUnit, ICountable
{
    
    protected ReactiveProperty<int> _count;
    public int Count => _count.Value;
    public override void Attack(GameUnit target)
    {
        // Implement attack logic here
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