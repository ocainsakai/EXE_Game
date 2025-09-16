using DG.Tweening;
using UnityEngine;

public class CardAnimationManager : MonoBehaviour
{
    private CardView cardView;
    public CardView CardView => cardView ??= GetComponentInChildren<CardView>();
    public void Moveup()
    {
        cardView.transform.DOLocalMoveY(10, 0.25f);
    }
    public void ResetZero()
    {
        cardView.transform.DOLocalMove(Vector3.zero, 0.25f);
    }
}
