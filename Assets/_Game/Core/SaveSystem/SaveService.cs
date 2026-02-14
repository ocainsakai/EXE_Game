using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using _Game.Core.SaveSystem.Serializers;

namespace _Game.Core.SaveSystem
{
    public class SaveService : ISaveService
    {
        private static SaveService _instance;
        public static SaveService Instance => _instance ??= new SaveService();

        private readonly string _savePath;
        private readonly ISaveSerializer _jsonSerializer;
        private readonly ISaveSerializer _csvSerializer;
        
        private GameSaveData _currentSave;

        private SaveService()
        {
            _savePath = Path.Combine(Application.persistentDataPath, "saves", "gamesave.json");
            _jsonSerializer = new JsonSaveSerializer();
            _csvSerializer = new CsvSaveSerializer();
            
            LoadAll();
        }

        private void LoadAll()
        {
            if (File.Exists(_savePath))
            {
                try
                {
                    string content = File.ReadAllText(_savePath);
                    _currentSave = _jsonSerializer.Deserialize<GameSaveData>(content);
                }
                catch (Exception e)
                {
                    Debug.LogError($"SaveService: Failed to load save file: {e.Message}");
                    _currentSave = new GameSaveData();
                }
            }
            else
            {
                _currentSave = new GameSaveData();
            }
        }

        public void Save<T>(string key, T data)
        {
            // Simple key-based routing to GameSaveData fields
            // In a more complex system, we'd use reflection or a dictionary of objects
            switch (key.ToLower())
            {
                case "progress":
                    if (data is PlayerProgressData p) _currentSave.progress = p;
                    break;
                case "deck":
                    if (data is DeckSaveData d) _currentSave.deck = d;
                    break;
                case "map":
                    if (data is MapSaveStateData m) _currentSave.map = m;
                    break;
                case "settings":
                    if (data is SettingsSaveData s) _currentSave.settings = s;
                    break;
                default:
                    Debug.LogWarning($"SaveService: Unknown save key '{key}'");
                    break;
            }
        }

        public T Load<T>(string key, T defaultValue = default)
        {
            object result = null;
            switch (key.ToLower())
            {
                case "progress": result = _currentSave.progress; break;
                case "deck": result = _currentSave.deck; break;
                case "map": result = _currentSave.map; break;
                case "settings": result = _currentSave.settings; break;
            }

            return result is T typedResult ? typedResult : defaultValue;
        }

        public bool HasKey(string key)
        {
            return Load<object>(key) != null;
        }

        public void Delete(string key)
        {
             // Reset to default
            switch (key.ToLower())
            {
                case "progress": _currentSave.progress = new(); break;
                case "deck": _currentSave.deck = new(); break;
                case "map": _currentSave.map = new(); break;
                case "settings": _currentSave.settings = new(); break;
            }
        }

        public void DeleteAll()
        {
            _currentSave = new GameSaveData();
            if (File.Exists(_savePath)) File.Delete(_savePath);
        }

        public void Flush()
        {
            string dir = Path.GetDirectoryName(_savePath);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            string content = _jsonSerializer.Serialize(_currentSave);
            File.WriteAllText(_savePath, content);
            Debug.Log($"SaveService: Saved to disk at {_savePath}");
        }

        public void ExportToFile(string filePath, SaveFormat format)
        {
            ISaveSerializer serializer = format == SaveFormat.Json ? _jsonSerializer : _csvSerializer;
            string content = serializer.Serialize(_currentSave);
            File.WriteAllText(filePath, content);
        }

        public void ImportFromFile(string filePath, SaveFormat format)
        {
            if (!File.Exists(filePath)) return;
            
            string content = File.ReadAllText(filePath);
            ISaveSerializer serializer = format == SaveFormat.Json ? _jsonSerializer : _csvSerializer;
            
            var imported = serializer.Deserialize<GameSaveData>(content);
            if (imported != null)
            {
                _currentSave = imported;
                Flush();
            }
        }
    }
}
