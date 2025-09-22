using CardSystem;
using DG.Tweening;
using System;
using UnityEngine;

public class Card : MonoBehaviour, ICard
{
    public static bool CanSelect;
    public static Vector3 discardPile;
    private CardData cardData;
    public CardData CardData => cardData;
    private CardAnimationManager _cardAnimation;
    public CardAnimationManager cardAnimation => _cardAnimation ??= GetComponent<CardAnimationManager>();
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
    public void SetData(CardData data)
    {
        this.cardData = data;
        cardAnimation.CardView.SetArt(data.Art, this);

    }

    public void MoveTo(Transform target)
    {
        transform.DOMove(target.position, 0.25f);

    }
    public void MoveTo(Vector3 target)
    {
        transform.DOMove(target, 0.25f);
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
