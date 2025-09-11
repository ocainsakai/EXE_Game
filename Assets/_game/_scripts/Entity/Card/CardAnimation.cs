using DG.Tweening;
using UnityEngine;

public class CardAnimation : MonoBehaviour
{
    [SerializeField] Transform model;   
    public void Moveup()
    {
        model.DOLocalMoveY(10, 0.25f);
    }
    public void ResetZero()
    {
        model.DOLocalMove(Vector3.zero, 0.25f);
    }
}
