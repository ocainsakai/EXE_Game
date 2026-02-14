using System;

namespace _Game.Core.SaveSystem.Serializers
{
    public interface ISaveSerializer
    {
        string Serialize(object data);
        T Deserialize<T>(string content);
        string FileExtension { get; }
    }
}
