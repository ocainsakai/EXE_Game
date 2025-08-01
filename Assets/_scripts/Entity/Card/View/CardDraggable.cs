using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardDraggable : MonoBehaviour
{
    [SerializeField] private CardSwapper _cardSwapper;
    private Vector3 _startPos;

    private void Awake()
    {
        _cardSwapper = GetComponent<CardSwapper>();
        var inputDecider = GetComponent<CardInputDecider>();
        if (inputDecider != null)
        {
            inputDecider.OnBeginDrag += OnBeginDrag;
            inputDecider.OnDragging += OnDrag;
            inputDecider.OnEndDrag += OnEndDrag;
        }   
    }
    public void OnBeginDrag()
    {
        _startPos = transform.position;
        transform.SetAsLastSibling();
    }

    public void OnDrag(Vector2 delta)
    {
        GetComponent<RectTransform>().anchoredPosition += delta / GetComponentInParent<Canvas>().scaleFactor;
    }

    public void OnEndDrag()
    {
        var targetCard = GetCardUnderPointer();
        if (targetCard != null && targetCard != gameObject)
        {
           _cardSwapper.SwapCards(targetCard, _startPos);
        }
        else
        {
            transform.DOMove(_startPos, 0.25f).SetEase(Ease.InQuad);
        }
    }

    private CardSwapper GetCardUnderPointer()
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
            var card = r.gameObject.GetComponentInParent<CardSwapper>();
            if (card && card.gameObject != gameObject)
            {
                Debug.Log($"Found target card: {card.dataHolder.Data.Value.name}");
                return card;
            }
        }
        return null;
    }
}
