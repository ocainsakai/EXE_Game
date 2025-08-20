using System;
using UnityEngine;

public class SwipeDetection : MonoBehaviour
{
    private InputManager inputManager;

    [SerializeField]
    private float minSwipeDistance = 0.2f; // Minimum distance to consider a swipe
    [SerializeField]
    private float maximunTime = 1f;
    [SerializeField]
    private float dragThreshsold = 0.1f; // Minimum time to consider a swipe
    [SerializeField, Range(0f, 1f)]
    private float directionThreshold = 0.9f; // Threshold for swipe detection

    public delegate void PressEvent(Vector2 direction);
    public event PressEvent OnSwipeDetected;
    public event PressEvent OnDragging;


    private Vector2 dragTouchPosition;
    private Vector2 startTouchPosition;
    private float startTouchTime;
    private Vector2 endTouchPosition;
    private float endTouchTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        inputManager = InputManager.Instance;
    }

    private void OnEnable()
    {
        inputManager.OnStartTouch += SwipeStart;
        inputManager.OnEndTouch += SwipeEnd;
        inputManager.OnTouch += OnDrag;
    }
    private void OnDisable()
    {
        inputManager.OnStartTouch -= SwipeStart;
        inputManager.OnEndTouch -= SwipeEnd;
        inputManager.OnTouch -= OnDrag;
    }
    private void OnDrag(Vector2 position, float time)
    {
        if (time - startTouchTime >= dragThreshsold)
        {
            var dragDirection = position - dragTouchPosition;
            dragDirection.Normalize();
            OnDragging?.Invoke(DetermineDirection(dragDirection));
        }
        dragTouchPosition = position;
    }
    private void SwipeEnd(Vector2 position, float time)
    {
        endTouchPosition = position;
        endTouchTime = time;
        DetectSwipe();
    }

    private void SwipeStart(Vector2 position, float time)
    { 
        startTouchPosition = position;
        startTouchTime = time;

    }
    private Vector2 DetermineDirection(Vector2 swipeDirection)
    {
        if (Vector2.Dot(swipeDirection, Vector2.up) > directionThreshold)
        {
            return Vector2.up;
        }
        else if (Vector2.Dot(swipeDirection, Vector2.down) > directionThreshold)
        {
            return Vector2.down;
        }
        else if (Vector2.Dot(swipeDirection, Vector2.left) > directionThreshold)
        {
            return Vector2.left;
        }
        else if (Vector2.Dot(swipeDirection, Vector2.right) > directionThreshold)
        {
            return Vector2.right;
        }
        return Vector2.zero;
    }

    private void DetectSwipe()
    {
        if (Vector2.Distance(startTouchPosition, endTouchPosition) >= minSwipeDistance &&
            (endTouchTime - startTouchTime) <= maximunTime)
        {
            Vector2 swipeDirection = endTouchPosition - startTouchPosition;
            swipeDirection.Normalize();
            OnSwipeDetected?.Invoke(DetermineDirection(swipeDirection));
           
        }
    }
    
}
