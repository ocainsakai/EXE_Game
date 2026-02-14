using UnityEngine;
using _Game.Core.DI;
using _Game.Core.SaveSystem;
using BulletHellTemplate;

namespace _Game.Core.Infrastructure
{
    public class GameBootstrapper : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;
        
        private void Awake()
        {
            // Register Core Services
            var saveService = SaveService.Instance;
            DIContainer.Global.Register<ISaveService>(saveService);
            DIContainer.Global.Register<GameManager>(gameManager);
            
            // Deck System
            var deckManager = new _Game.Core.Systems.DeckManager(saveService);
            DIContainer.Global.Register<_Game.Core.Systems.DeckManager>(deckManager);
            
            // Add more core services here
            Debug.Log("GameBootstrapper: Core services registered.");
        }
    }
}
