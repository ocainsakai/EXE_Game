
public interface IHexOccupant<T> where T : IHex<T>
{
    T CurrentHex { get; }
    void SetHex(T newHex);
    void OnEnter();
    void OnLeave();
}
public enum HexContentType
{
    None,
    Player,
    Enemy,
    Boss
}
