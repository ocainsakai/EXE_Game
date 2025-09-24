using CardSystem;
using System;

public class Card
{
    public static bool CanSelect;
    private CardData cardData;
    public CardData CardData => cardData;
   
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

    public Card(CardData cardData)
    {
        this.cardData = cardData;
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
