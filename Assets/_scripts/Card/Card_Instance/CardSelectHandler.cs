using DG.Tweening;
using UniRx;
using UnityEngine;

public class CardSelectHandler : MonoBehaviour
{

    public static bool CanSelect = true;
    public Subject<Card> OnClick =  new Subject<Card>();
    
    private bool isSelecting = false;
    private Card cardCtrl => GetComponent<Card>();
    public void OnMouseDown()
    {
        Debug.Log("can select: " + CanSelect);
        if (CanSelect && !isSelecting)
        {
            isSelecting = true;
            transform.DOLocalMoveY(0.5f, 0.2f);
            OnClick?.OnNext(cardCtrl);
        } else if (isSelecting)
        {
            isSelecting = false;
            transform.DOLocalMoveY(0f, 0.2f);
            OnClick?.OnNext(cardCtrl);
        }
    }
}
