using System;

namespace _Game.Core.SaveSystem
{
    public interface ISaveService
    {
        void Save<T>(string key, T data);
        T Load<T>(string key, T defaultValue = default);
        bool HasKey(string key);
        void Delete(string key);
        void DeleteAll();
        void Flush();  // Persist current in-memory data to disk
        
        // Export/Import for analytics or balance data
        void ExportToFile(string filePath, SaveFormat format);
        void ImportFromFile(string filePath, SaveFormat format);
    }

    public enum SaveFormat 
    { 
        Json, 
        Csv 
    }
}
