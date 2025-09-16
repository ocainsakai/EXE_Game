using UnityEngine;

public class PlayerStateLoader : MonoBehaviour
{
    [SerializeField] PlayerManager manager;
    [SerializeField] public PlayerStateSO basic;

    public void LoadConfig()
    {
        Debug.Log(basic.state);
        if (basic == null) return;
        manager.CurrentState = basic.state;
    }
}
