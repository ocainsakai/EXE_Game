using UnityEngine;

[CreateAssetMenu(fileName = "CardLayoutSetting", menuName = "Scriptable Objects/CardLayoutSetting")]
public class CardLayoutSettings : ScriptableObject
{

    [Header("Layout Configuration")]
    public Alignment alignment = Alignment.Center;
    public int totalWidth = 1000;
    public float spacing = 50f;
}
public enum Alignment { None, Left, Center, Right }
