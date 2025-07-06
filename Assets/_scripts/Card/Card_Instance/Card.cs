using UnityEngine;


[RequireComponent(typeof(CardView))]
public class Card : MonoBehaviour
{
    public CardData Data {  get; private set;} 
    private CardView cardView;
    private CardSelectHandler cardSelectHandler;

    public bool IsFront { get; private set; }

    private void Awake()
    {
        cardView = GetComponent<CardView>();
        cardSelectHandler = GetComponent<CardSelectHandler>();
    }
    
    public void Initialize(CardData data, Sprite CardBack)
    {
        this.Data = data;
        cardView.SetView(Data.Art, CardBack);
    }
    public void SetFace(bool isFront)
    {
        IsFront = isFront;
        cardView.Artwork.sprite = IsFront ? cardView.CardFront : cardView.CardBack;
    }
    public void Unselect()
    {
        cardSelectHandler.isSelecting = false;
    }
    public void Select()
    {
        cardSelectHandler.isSelecting = true;
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