using CardSystem;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
public class CardView : MonoBehaviour, IPointerClickHandler, IDragHandler, IDropHandler
{
    private CardData _cardData;
    public CardData cardData => _cardData;
    public static Action<CardView> onCardClicked;
    public static Action<CardView> onCardDrag;
    public static Action<CardView> onCardDrop;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("clicked");
        onCardClicked?.Invoke(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("darg");
        onCardDrag?.Invoke(this);
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("drop");
        onCardDrop?.Invoke(this);
    }

    internal void SetData(CardData data)
    {
        _cardData = data;
    }
}
