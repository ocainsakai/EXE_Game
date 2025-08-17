
using UnityEngine;

[CreateAssetMenu(fileName = "NewCardData", menuName = "Scriptable Objects/CardData", order = 1)]
public class CardData : ScriptableObject
{
	public int Cost;
	public string Name;
	public string Description;
	
}