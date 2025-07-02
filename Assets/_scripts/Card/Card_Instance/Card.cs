using System;
using UnityEngine;


[RequireComponent(typeof(CardView))]
public class Card : MonoBehaviour
{
    public static bool CanSelect = true;   
    public CardData Data {  get; private set;} 
    private CardView cardView;

    public bool IsFront { get; private set; }

    private void Awake()
    {
        cardView = GetComponent<CardView>();
      
    }
    
    public void Initialize(CardData data, Sprite CardBack)
    {
        this.Data = data;
        cardView.SetView(Data.Art, CardBack);
    }
    public void SetFace(bool isFront)
    {
        IsFront = isFront;
        cardView.Artwork.sprite = cardView.CardFront;
    }
}
public enum CardState
{
    None,
    Hold,
    Select,
    Play,
    Score
}