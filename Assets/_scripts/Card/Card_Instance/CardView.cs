using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CardView : MonoBehaviour
{
    public SpriteRenderer Artwork;
    public Sprite CardFront { get; private set; }
    public Sprite CardBack { get; private set; }
    public Action OnClicked;
    private void Awake()
    {
        Artwork = GetComponent<SpriteRenderer>();
    }
    public void SetView(Sprite front, Sprite back)
    {
        CardFront = front;
        CardBack = back;
        Artwork.sprite = CardBack;
    }
   
    public void OnMouseDown()
    {
        OnClicked?.Invoke();
    }
}