using CardSystem;
using DG.Tweening;
using System;
using UnityEngine;

public class CardController : MonoBehaviour
{
    public static bool CanSelect;
    [SerializeField] private CardView cardView;
    private CardData cardData;
    public CardData CardData => cardData;
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
    public Action onSelectChange;
    public Transform discardPile;
    public void SetData(CardData data, Transform discardPile)
    {
        this.cardData = data;
        this.discardPile = discardPile;
        cardView.SetArt(data.Art, this);

    }
    public void Discard()
    {
        MoveTo(discardPile);
    }
    public void DrawCard(Transform room)
    {
        MoveTo(room);
    }
    public void MoveTo(Transform target)
    {
        // move to discard pile
        transform.DOMove(target.position, 0.25f);
    }
    public void Select()
    {
        IsSelecting = true;
    }
    public void Unselect()
    {
        IsSelecting = false;
    }
}
