public class CardState
{
    public readonly SerializableGuid CardStateId;
    public CardSDData CardSDData;
    public bool IsSelected;

    public CardState(CardSDData cardSDData)
    {
        CardStateId = SerializableGuid.NewGuid();
        CardSDData = cardSDData;
        IsSelected = false;
    }
}