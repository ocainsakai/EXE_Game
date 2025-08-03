using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardInputDecider : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public enum CardState { Idle, Selected, Dragging }

    public event Action OnClick;        
    public event Action OnBeginDrag;    
    public event Action<Vector2> OnDragging; 
    public event Action OnEndDrag;      

    public bool CanSelect { get; set; } = false;
    //private float _pointerDownTime;
    private Vector2 _pointerDownPos;
    private bool _isDragging;

    //private const float _dragThreshold = 0.15f;
    private const float _dragDistanceThreshold = 10f;

    public CardState CurrentState { get; private set; } = CardState.Idle;
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!CanSelect)
        {
            return;
        }
        //_pointerDownTime = Time.time;
        _pointerDownPos = eventData.position;

        _isDragging = false;
        CurrentState = CardState.Idle;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!CanSelect)
        {
            return;
        }
        if (!_isDragging)
        {
            float dist = Vector2.Distance(eventData.position, _pointerDownPos);
            if (dist < _dragDistanceThreshold)
            {
                CurrentState = CardState.Selected;
                OnClick?.Invoke();
                return;
            }
        }
        if (_isDragging)
        {
            CurrentState = CardState.Idle;
            OnEndDrag?.Invoke();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!CanSelect)
        {
            return;
        }
        if (!_isDragging)
        {
            float dist = Vector2.Distance(eventData.position, _pointerDownPos);
            if (dist >= _dragDistanceThreshold)
            {
                // Bắt đầu kéo
                _isDragging = true;
                CurrentState = CardState.Dragging;
                OnBeginDrag?.Invoke();
            }
        }

        if (_isDragging)
        {
            OnDragging?.Invoke(eventData.delta);
        }
    }
}
