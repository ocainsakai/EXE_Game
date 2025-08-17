using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Cysharp.Threading.Tasks;
public class Card : MonoBehaviour
{
    [SerializeField] public CardInputDecider inputDecider;
    [SerializeField] public CardSelecter selecter;
    //[SerializeField] public CardDraggable draggable;
    [SerializeField] public CardDataHolder dataHolder;
    public CardSDData Data => dataHolder.Data.Value;

    private void Awake()
    {
        if (dataHolder == null)
        { 
            dataHolder = GetComponent<CardDataHolder>();
        }
        if (inputDecider == null)
        {
            inputDecider = GetComponent<CardInputDecider>();
        }
        if (selecter == null)
        {
            selecter = GetComponent<CardSelecter>();
        }
       
        dataHolder.Data.Subscribe(Data => UpdateImage(Data?.Art)).AddTo(this);
        
    }
    public void OnDisable()
    {
        inputDecider.CanInteract = false;
        selecter.CanSelect = false;
    }
    public void ResetState()
    {
        selecter.Deselect();
    }
    public void TurnOnInput()
    {
        inputDecider.CanInteract = true;
        selecter.CanSelect = true;
    }
    public void UpdateImage(Sprite sprite)
    {
        GetComponentInChildren<Image>().sprite = sprite;
    }

    public async UniTask Attack(Enemy enemy)
    {
        // Assuming the card has an Attack method
        if (Data != null && enemy != null)
        {
            await enemy.TakeDamage(10);
        }
    }
    public async UniTask Attack(Enemy enemy, int mult)
    {
        if (Data != null && enemy != null)
        {
            await enemy.TakeDamage(Data.Rank * mult);
        }
    }
}