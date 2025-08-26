using System;
using System.Collections.Generic;

[Serializable]
public class DeckState
{
    public List<SerializableGuid> DeckOrder; // list of CardId, top = index 0 or last (decide)
    public List<SerializableGuid> Hand; // CardId in hand order
    public List<SerializableGuid> DiscardPile;
}
