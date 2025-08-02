using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardDraggable : MonoBehaviour
{
    public static event Action<Card, Card> OnCardSwapped;
    private Vector3 _startPos;

    private void Awake()
    {
        var inputDecider = GetComponent<CardInputDecider>();
        if (inputDecider != null)
        {
            inputDecider.OnBeginDrag += OnBeginDrag;
            inputDecider.OnDragging += OnDrag;
            inputDecider.OnEndDrag += OnEndDrag;
        }   
    }
    private void OnBeginDrag()
    {
        _startPos = transform.position;
        transform.SetAsLastSibling();
    }

    private void OnDrag(Vector2 delta)
    {
        GetComponent<RectTransform>().anchoredPosition += delta / GetComponentInParent<Canvas>().scaleFactor;
    }

    private void OnEndDrag()
    {
        var targetCard = GetCardUnderPointer();
        if (targetCard != null && targetCard != gameObject)
        {
            OnCardSwapped?.Invoke(GetComponent<Card>(), targetCard);
        }
        else
        {
            transform.DOMove(_startPos, 0.25f).SetEase(Ease.InQuad);
        }
    }

    private Card GetCardUnderPointer()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (var r in results)
        {
            // raycast all will return game objects that have a RaycastTarget component
            // we need to check if the game object is a CardSwapper
            var card = r.gameObject.GetComponentInParent<Card>();
            if (card && card.gameObject != gameObject)
            {
                return card;
            }
        }
        return null;
    }
}
