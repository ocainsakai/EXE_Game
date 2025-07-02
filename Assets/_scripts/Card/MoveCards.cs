using System.Collections.Generic;
using UnityEngine;

public class MoveCards : MonoBehaviour
{
    public Transform playerHand;
    public Transform discardPiles;
    public Transform deckPiles;
    public Transform scoreBoard;
    public enum Place
    {
        Hand,
        DiscardPile,
        Deck,
        ScoreBoard
    }
    public void MoveTo(Place place, IEnumerable<Card> cards)
    {

    }

    
}
