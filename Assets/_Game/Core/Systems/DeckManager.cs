using System.Collections.Generic;
using System.Linq;
using _Game.Core.Data;
using _Game.Core.SaveSystem;
using _Game.Core.DI;
using UnityEngine;

namespace _Game.Core.Systems
{
    public class DeckManager
    {
        private UserDeckData _userDeckData;
        private ISaveService _saveService;
        
        // All available cards in the game (database)
        private Dictionary<SerializableGuid, CardData> _cardDatabase = new Dictionary<SerializableGuid, CardData>();

        public DeckManager(ISaveService saveService)
        {
            _saveService = saveService;
            LoadDatabase();
            LoadUserData();
        }

        private void LoadDatabase()
        {
            var cards = Resources.LoadAll<CardData>("Cards");
            foreach (var card in cards)
            {
                if (!_cardDatabase.ContainsKey(card.Id))
                {
                    _cardDatabase.Add(card.Id, card);
                }
                else
                {
                    Debug.LogWarning($"Duplicate Card ID found: {card.Id} on {card.name}");
                }
            }
            Debug.Log($"DeckManager: Loaded {_cardDatabase.Count} cards into database.");
        }

        private void LoadUserData()
        {
            var saveData = _saveService.GetData();
            if (saveData != null && saveData.cardCollection != null)
            {
                _userDeckData = saveData.cardCollection;
            }
            else
            {
                _userDeckData = new UserDeckData();
                saveData.cardCollection = _userDeckData;
            }
        }

        public void SaveUserData()
        {
            _saveService.Save();
        }

        public List<CardData> GetOwnedCards()
        {
            return _userDeckData.ownedCardIds
                .Select(id => _cardDatabase.GetValueOrDefault(id))
                .Where(c => c != null)
                .ToList();
        }

        public List<CardData> GetCurrentDeck()
        {
            return _userDeckData.currentDeckIds
                .Select(id => _cardDatabase.GetValueOrDefault(id))
                .Where(c => c != null)
                .ToList();
        }

        public void AddCardToCollection(SerializableGuid cardId)
        {
            if (!_userDeckData.ownedCardIds.Contains(cardId))
            {
                _userDeckData.ownedCardIds.Add(cardId);
                SaveUserData();
            }
        }

        public bool AddCardToDeck(SerializableGuid cardId)
        {
            if (!_userDeckData.ownedCardIds.Contains(cardId)) return false;
            
            _userDeckData.currentDeckIds.Add(cardId);
            SaveUserData();
            return true;
        }

        public bool RemoveCardFromDeck(SerializableGuid cardId)
        {
            if (_userDeckData.currentDeckIds.Contains(cardId))
            {
                _userDeckData.currentDeckIds.Remove(cardId);
                SaveUserData();
                return true;
            }
            return false;
        }
        
        public CardData GetCardData(SerializableGuid id)
        {
            return _cardDatabase.GetValueOrDefault(id);
        }
    }
}
