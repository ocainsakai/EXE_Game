using UnityEngine;

public class PlayerCardHolder : MonoBehaviour
{
    public CardContainer Deck;
    public CardContainer DiscardPiles;
    public CardContainer Hand;
    public CardContainer ScoreBoard;

  

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Deck.Cards.Count <=0)
        {
           
        }

    }
   
}

