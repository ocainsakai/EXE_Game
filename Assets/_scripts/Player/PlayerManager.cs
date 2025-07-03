using UnityEngine;
using UniRx;
using System;
public class PlayerManager : MonoBehaviour
{
    public PlayerConfigs playerConfigs;
    public CardManager cardManager;
    public InputManager inputManager;

    private void Awake()
    {
        inputManager.Draw.Subscribe(_ => DrawHand());
        inputManager.Discard.Subscribe(_ => Discard());
        inputManager.Play.Subscribe(_ => Play());
        inputManager.Sort.Subscribe(_ => Sort());
        cardManager.cardsContext.selected.ObserveCountChanged()
            .Subscribe(x => CardSelectHandler.CanSelect = x < playerConfigs.SelectSize);
    }

    private void Sort()
    {
        Debug.Log("sorting");
        cardManager.NextSortType();
    }

    private void Play()
    {
        
    }

    public void DrawHand()
    {
        cardManager.DrawHand(DrawAmount);
    }
    public void Discard()
    {
        cardManager.Discard();
        DrawHand();
    }
    private int  DrawAmount => playerConfigs.HandSize - cardManager.cardsContext.hand.Count;
}
