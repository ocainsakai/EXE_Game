using CardSystem;
using UnityEngine;
public class CardView : MonoBehaviour
{
    [SerializeField]
    private CardColliderHandler cardColliderHandler;
    [SerializeField] 
    private SpriteRenderer spriteRenderer;
    internal void SetArt(Sprite Art)
    {
        spriteRenderer.sprite = Art;
    }

    private void OnEnable()
    {
        cardColliderHandler.onCardClicked += CardClickHandle;
    }
    private void OnDisable()
    {
        cardColliderHandler.onCardClicked -= CardClickHandle;        
    }

    private void CardClickHandle()
    {
    }

}
