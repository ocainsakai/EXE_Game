using DG.Tweening;
using UnityEngine;

public class CardSwapper : MonoBehaviour
{
    [SerializeField] public CardDataHolder dataHolder;

    public float duration = 0.25f;
    public Ease easing = Ease.OutQuad;
    public void SwapCards(CardSwapper target)
    {
        var _startPos = transform.position;
        Debug.Log($"Swapping cards: {dataHolder.Data.Value.name} with {target.dataHolder.Data.Value.name}");
        transform.DOMove(target.transform.position, duration).SetEase(easing);
        target.transform.DOMove(_startPos, duration).SetEase(easing);
    }
    public void SwapCards(CardSwapper target, Vector2 _startPos)
    {
        Debug.Log($"Swapping cards: {dataHolder.Data.Value.name} with {target.dataHolder.Data.Value.name}");
        transform.DOMove(target.transform.position, duration).SetEase(easing);
        target.transform.DOMove(_startPos, duration).SetEase(easing);
    }
}
