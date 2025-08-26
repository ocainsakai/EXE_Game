using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Card : MonoBehaviour, IPointerClickHandler, IDragHandler, IDropHandler
{
    public CardState State { get; private set; }
    public static Action<Card> onCardClicked;
    public static Action<Card> onCardDrag;
    public static Action<Card> onCardDrop;

    public void Init(CardSDData data)
    {
        this.State = new CardState(data);
        UpdateImage(data.Art);
    }

    public void UpdateImage(Sprite Art)
    {
        GetComponentInChildren<Image>().sprite = Art;
    }

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
}
