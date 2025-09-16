using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardColliderHandler : MonoBehaviour, IPointerClickHandler, IDragHandler, IDropHandler
{
    public Action onCardClicked;
    public Action onCardDrag;
    public Action onCardDrop;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("clicked");
        onCardClicked?.Invoke();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("darg");
        onCardDrag?.Invoke();
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("drop");
        onCardDrop?.Invoke();
    }

}
