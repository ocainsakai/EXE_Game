using Map;
using System;
using UnityEngine;

public class HexController : MonoBehaviour
{
    public static Action<HexRuntime> OnHexClicked;
    private HexDataHolder _dataHolder;
    public HexRuntime Data => _dataHolder.Data;

    private void Awake()
    {
        _dataHolder = GetComponent<HexDataHolder>();
        if (_dataHolder == null)
        {
            Debug.LogError("HexController requires a HexDataHolder component.");
        }
    }
    private void OnMouseDown()
    {
        Debug.Log("Hex Clicked");
        if (UIState.IsBlocking) return;
        
        OnHexClicked?.Invoke(Data);
    }
}
