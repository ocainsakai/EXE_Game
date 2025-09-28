using CardSystem;
using System;

public class Card
{
    public static bool CanSelect;
    private CardData _cardData;
    public CardData CardData => _cardData;

    public Action onSelectChange;
    private bool isSelecting;
    public bool IsSelecting
    {
        get => isSelecting;
        set
        {
            isSelecting = value;
            onSelectChange?.Invoke();

        }
    }

    public Card(CardData data)
    {
        _cardData = data;
    }

    public void OnCardClickHandle()
    {
        if (!IsSelecting && !CanSelect) return;

        else if (!IsSelecting && CanSelect)
        {
            IsSelecting = true;
        }
        else if (IsSelecting)
        {
            IsSelecting = false;
        }
    }
}
