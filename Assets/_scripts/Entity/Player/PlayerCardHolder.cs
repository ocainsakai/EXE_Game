using UnityEngine;

public class PlayerCardHolder : MonoBehaviour
{
    public CardContainer Deck;
    public CardContainer DiscardPiles;
    public CardContainer Hand;
    public CardContainer ScoreBoard;

  

    public void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            
            
            if (Deck.Cards.Count > 0)
            {
                addhand();
            } else
            {
                foreach (Transform child in Deck.transform)
                {
                    Deck.Add(child.GetComponent<Card>());
                }
            }
        }
    }
    private void addhand()
    {
        var card = Deck.Cards[0];
        Hand.Add(Deck.Cards[0]);
        Deck.Remove(card);
    }
}
