using System;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    [SerializeField] TextDisplay roundText;
    private int _currentRound;
    private int _currentTurn;

    public int TurnsOnRound { get; private set; }
    public int CurrentRound
    {
        get => _currentRound;
        set
        {
            if (_currentRound == value) return;
            _currentRound = value;
            onRoundChanged?.Invoke();
        }
    }

    public int CurrentTurn
    {
        get => _currentTurn;
        set
        {
            if (_currentTurn == value) return;
            _currentTurn = value;
            onTurnChanged?.Invoke();
        }
    }
    public static Action onTurnChanged;
    public static Action onRoundChanged;

    public void SetTurnOnRound(int max)
    {
        TurnsOnRound = max;
    }
    public void NextTurn()
    {
        CurrentTurn++;
        if (CurrentTurn > TurnsOnRound)
        {
            NextRound();
            CurrentTurn = 0;
        }
    }
    public void NextRound()
    {
        CurrentRound++;
        string content = "Round: " + CurrentRound;
        roundText.UpdateContent(content);
    }
    public void Initialze()
    {
        _currentRound = 0;
        _currentTurn = 0;
    }
}
