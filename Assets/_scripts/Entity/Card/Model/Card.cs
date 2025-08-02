using UnityEngine;
using UnityEngine.UI;
using UniRx;
public class Card : GameUnit
{

    [SerializeField] CardDataHolder dataHolder;
    public CardSDData Data => dataHolder.Data.Value;

    private void Awake()
    {
        if (dataHolder == null)
        { 
            dataHolder = GetComponent<CardDataHolder>();
        }
        dataHolder.Data.Subscribe(Data => UpdateImage(Data?.Art)).AddTo(this);
        
    }
    public void UpdateImage(Sprite sprite)
    {
        GetComponentInChildren<Image>().sprite = sprite;
    }
    public override void Attack(GameUnit target)
    {
        // Implement attack logic here
        target.HP -= this.ATK;
    }
    
}