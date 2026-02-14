using UnityEngine;
using UnityEngine.UI;
using _Game.Core.SaveSystem;

namespace _Game.Core.Flow.Panels
{
    public class MainMenuPanel : GamePanel
    {
        public override GameFlowState State => GameFlowState.MainMenu;

        [Header("UI Elements")]
        [SerializeField] private Button continueButton;
        [SerializeField] private Button newGameButton;

        private GameFlowController _flowController;

        private void Awake()
        {
            _flowController = GetComponentInParent<GameFlowController>();
            
            if (newGameButton != null)
                newGameButton.onClick.AddListener(OnNewGameClicked);
            
            if (continueButton != null)
                continueButton.onClick.AddListener(OnContinueClicked);
        }

        public override void OnEnter(object data = null)
        {
            base.OnEnter(data);
            RefreshUI();
        }

        private void RefreshUI()
        {
            // Check if map save exists
            bool hasSave = SaveService.Instance.HasKey("map");
            if (continueButton != null)
                continueButton.interactable = hasSave;
        }

        private void OnNewGameClicked()
        {
            // Reset map
            SaveService.Instance.Delete("map");
            SaveService.Instance.Flush();
            
        }

        private void OnContinueClicked()
        {
        }
    }
}
