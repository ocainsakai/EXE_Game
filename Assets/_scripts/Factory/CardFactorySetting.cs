using UnityEngine;

[CreateAssetMenu(fileName = "CardFactorySetting", menuName = "Scriptable Objects/CardFactorySetting")]
public class CardFactorySetting : ScriptableObject
{
    public int _poolSize = 52;
    public Card _cardPrefab;
}
