using System;
using UnityEngine;

public class Buuble : MonoBehaviour
{
    public static Action Break;
    private void OnMouseDown()
    {
        Break?.Invoke();
        Destroy(gameObject);        
    }
    
}
