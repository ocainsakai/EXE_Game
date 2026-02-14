using System;
using System.Collections.Generic;

namespace _Game.Core.Data
{
    [Serializable]
    public class UserDeckData
    {
        public List<SerializableGuid> ownedCardIds = new List<SerializableGuid>();
        public List<SerializableGuid> currentDeckIds = new List<SerializableGuid>();

        public UserDeckData()
        {
            ownedCardIds = new List<SerializableGuid>();
            currentDeckIds = new List<SerializableGuid>();
        }
    }
}
