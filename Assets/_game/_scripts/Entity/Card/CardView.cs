using DG.Tweening;
using UnityEngine;
public class CardView : MonoBehaviour, ICardClickable
{
    [SerializeField]
    private CardColliderHandler cardColliderHandler;
    [SerializeField]
    private CardAnimation cardAnimation;
    
    [SerializeField] 
    private SpriteRenderer spriteRenderer;
    private CardController cardController;
    public void SetArt(Sprite Art, CardController cardController)
    {
        spriteRenderer.sprite = Art;
        this.cardController = cardController;
    }

    private void OnEnable()
    {
        cardColliderHandler.onCardClicked += CardClickHandle;
    }
    private void OnDisable()
    {
        cardColliderHandler.onCardClicked -= CardClickHandle;        
    }

    public void CardClickHandle()
    {
        if (!cardController.IsSelecting && !CardController.CanSelect) return;

        else if (!cardController.IsSelecting && CardController.CanSelect)
        {
            cardColliderHandler.transform.DOLocalMoveY(0.5f, 0.25f);
            cardController.Select();
        }
        else if (cardController.IsSelecting) {
            cardColliderHandler.transform.DOLocalMoveY(0f, 0.25f);
            cardController.Unselect();
        }
    }

}
