using UnityEngine;


[RequireComponent(typeof(CardView))]
public class Card : MonoBehaviour
{
    // static state
    public static bool CanSelect = true;
    private float originY;
   
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
    }
    public void MoveUp()
    {
        originY = transform.localPosition.y;
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