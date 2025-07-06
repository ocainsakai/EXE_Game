using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class CardsMove : MonoBehaviour
{
    public Transform playerHand;
    public Transform discardPiles;
    public Transform deckPiles;
    public Transform scoreBoard;
    public enum Align
    {
        None,
        Center, Left, Right, Top, Bottom,
        TopLeft, BottomRight,
    }

    public void MoveToHand(IEnumerable<Card> cards)
    {
        Transform target = playerHand;
        MoveTo(cards, target, Align.Center);
    }
    public void MoveToDiscard(IEnumerable<Card> cards)
    {
        Transform target = discardPiles;
        MoveTo(cards, target, Align.None);
    }
    public void MoveToScore(IEnumerable<Card> cards)
    {
        Transform target = scoreBoard;
        MoveTo(cards, target, Align.Center);
    }
    private void MoveTo(IEnumerable<Card> cards, Transform target, Align align = Align.None)
    {
        var positions = CalculatePos(align, target.position, cards.Count());
        for (int i =0; i < cards.Count(); i++)
        {
            var item = cards.ElementAt(i);
            item.transform.SetParent(playerHand);
            item.SetFace(true);
            item.transform.DOMove(positions.ElementAt(i), 0.2f).SetEase(Ease.OutQuad);
        }
    }
    private IEnumerable<Vector3> CalculatePos(Align align, Vector3 pos, int count) {
        Vector3[] result = Enumerable.Repeat(pos, count).ToArray();
        //Debug.Log(result[result.Length - 1]);
        switch (align)
        {
            case Align.Center:
                float space = 1f;
                Vector3 startPos = pos + Vector3.left * (count / 2) * space;
                for (int i = 0; i < count; i++)
                {
                    var target = startPos + Vector3.right * space * i;
                    result[i] = target;
                }
                break;
        }
        return result;
    }
}
