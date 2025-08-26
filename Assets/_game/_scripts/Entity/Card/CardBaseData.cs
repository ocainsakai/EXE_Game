using UnityEngine;


public abstract class CardBaseData : ScriptableObject, IData
{
	public SerializableGuid CardID = SerializableGuid.NewGuid();
	public int Cost;
	public string Name;
	public string Description;

    public SerializableGuid ID => CardID;
}