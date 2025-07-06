using UnityEngine;
using UniRx;
using System;
using TMPro;
public class PlayerManager : MonoBehaviour
{
    public GamePhase gamePhase;
    public PlayerConfigs playerConfigs;
    public CardManager cardManager;
    public InputManager inputManager;
    public TextMeshPro playerText;
    public BattleManager battleManager;
    private void Awake()
    {
        BlindInput();
        cardManager.cardsContext.selected.ObserveCountChanged()
            .Subscribe(x => CardSelectHandler.CanSelect = x < playerConfigs.SelectSize);
        gamePhase.currentPhase.Subscribe(phase => {
            switch (phase)
            {
                case Phase.StartTurn:
                    DrawHand();
                    break;
                case Phase.PlayerTurn:
                    Play();
                    break;
                case Phase.EndTurn:
                    ResetBoard();
                    break;
            }
        });
        
    }

    private void BlindInput()
    {
        inputManager.Draw.Subscribe(_ => DrawHand());
        inputManager.Discard.Subscribe(_ => Discard());
        inputManager.Sort.Subscribe(_ => Sort());
    }
    private void ResetBoard()
    {
        cardManager.ResetBoard();
    }
    private void Sort()
    {
        cardManager.NextSortType();
    }

    private void Play()
    {
        cardManager.MoveToScore();
        battleManager.StartBattle();
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
