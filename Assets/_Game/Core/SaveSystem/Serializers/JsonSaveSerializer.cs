using UnityEngine;

namespace _Game.Core.SaveSystem.Serializers
{
    public class JsonSaveSerializer : ISaveSerializer
    {
        public string FileExtension => "json";

        public string Serialize(object data)
        {
            return JsonUtility.ToJson(data, true); // true for pretty print in dev, could be toggled
        }

        public T Deserialize<T>(string content)
        {
            return JsonUtility.FromJson<T>(content);
        }
    }
}
