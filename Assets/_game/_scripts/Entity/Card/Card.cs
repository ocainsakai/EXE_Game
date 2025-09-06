using CardSystem;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Card : MonoBehaviour, IPointerClickHandler, IDragHandler, IDropHandler
{

    public SerializableGuid ID;
    public CardData Data;
    public static Action<Card> onCardClicked;
    public static Action<Card> onCardDrag;
    public static Action<Card> onCardDrop;
    public bool IsSelected;

    public void SetData(CardData Data)
    {
        this.Data = Data;
        ID = SerializableGuid.NewGuid();
        GetComponentInChildren<Image>().sprite = Data.Art;
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
