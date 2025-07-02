using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
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
        Transform target = GetTarget(place);
        var positions = CalculatePos(target.position, cards.Count());
        for (int i =0; i < cards.Count(); i++)
        {
            var item = cards.ElementAt(i);
            item.transform.SetParent(playerHand);
            item.SetFace(true);
            item.transform.DOLocalMove(positions.ElementAt(i), 0.2f).SetEase(Ease.OutQuad);
        }
    }
    private Transform GetTarget(Place place)
    {
        switch (place)
        {
            case Place.Hand: return playerHand;
            case Place.DiscardPile: return discardPiles;
            case Place.Deck: return deckPiles;
            case Place.ScoreBoard: return scoreBoard;
        }
        return playerHand.transform;
    }
    private IEnumerable<Vector3> CalculatePos(Vector3 pos, int count) {

        List<Vector3> result = new List<Vector3>();
        float space = 1f;
        Vector3 startPos = pos + Vector3.left * (count/2) * space;
        for (int i = 0; i < count; i++)
        {
            var target = startPos + Vector3.right * space * i;
            result.Add(target);
        }
        return result;
    }
}
