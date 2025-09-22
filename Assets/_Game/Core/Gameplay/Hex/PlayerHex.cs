using UnityEngine;

public class PlayerHex : MonoBehaviour, IHexOccupant<Hex>
{
    private Hex currentHex;
    public Hex CurrentHex => currentHex;
    public void SetHex(Hex hex)
    {
        if (currentHex != null)
        {
            currentHex.transform.DetachChildren();
        }
        currentHex = hex;
        transform.SetParent(hex.transform, false);
        transform.localPosition = Vector3.zero;
    }

    public void OnEnter()
    {
    }

    public void OnLeave()
    {
    }
}
