using CardSystem;
using UnityEngine;

public class CardController : MonoBehaviour
{
    [SerializeField] private CardView cardView;
    [SerializeField] private CardAnimation cardAnimation;
    private CardData cardData;
    public CardData CardData => cardData;

    public void SetData(CardData data)
    {
        this.cardData = data;
        cardView.SetArt(data.Art);

    }
}
