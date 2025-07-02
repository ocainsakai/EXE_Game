using UnityEngine;

public class InputManager : MonoBehaviour
{
    BattleAction inputs;

    private void Awake()
    {
        inputs = new BattleAction();

        inputs.PlayerAction.Play.performed += ctx => Debug.Log("Play");
        inputs.PlayerAction.Draw.performed += ctx => Debug.Log("Draw");
        inputs.PlayerAction.Discard.performed += ctx => Debug.Log("Discard");

        inputs.Enable();
    }

    private void OnDestroy()
    {
        inputs.Disable();
        inputs.Dispose();
    }
}
