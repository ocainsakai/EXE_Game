using System;
using UniRx;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    BattleAction inputs;

    private void Awake()
    {
        inputs = new BattleAction();

        inputs.PlayerAction.Play.performed += ctx => Play.OnNext(Unit.Default);
        inputs.PlayerAction.Draw.performed += ctx => Draw.OnNext(Unit.Default);
        inputs.PlayerAction.Discard.performed += ctx => Discard.OnNext(Unit.Default);
        inputs.PlayerAction.Sort.performed += ctx => Sort.OnNext(Unit.Default);

        inputs.Enable();
    }
    public Subject<Unit> Play = new Subject<Unit>();
    public Subject<Unit> Draw = new Subject<Unit>();
    public Subject<Unit> Discard = new Subject<Unit>();
    public Subject<Unit> Sort = new Subject<Unit>();

    private void OnDestroy()
    {
        inputs.Disable();
        inputs.Dispose();
    }
}
