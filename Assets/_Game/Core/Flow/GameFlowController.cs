using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace _Game.Core.Flow
{
    public class GameFlowController : MonoBehaviour
    {
        [SerializeField] private List<GamePanel> panels;
        [SerializeField] private GameFlowState initialState = GameFlowState.MainMenu;

        private GamePanel _currentPanel;
        public GameFlowState CurrentState => _currentPanel != null ? _currentPanel.State : initialState;

        private void Awake()
        {
            _Game.Core.DI.DIContainer.Global.Register<GameFlowController>(this);
        }

        private void Start()
        {
            // Deactivate all panels except initial
            foreach (var panel in panels)
            {
                panel.gameObject.SetActive(false);
            }

            TransitionTo(initialState);
        }

        public void TransitionTo(GameFlowState newState, object data = null)
        {
            if (_currentPanel != null && _currentPanel.State == newState)
                return;

            Debug.Log($"GameFlowController: Transitioning to {newState}");

            if (_currentPanel != null)
            {
                _currentPanel.OnExit();
            }

            _currentPanel = panels.FirstOrDefault(p => p.State == newState);

            if (_currentPanel != null)
            {
                _currentPanel.OnEnter(data);
            }
            else
            {
                Debug.LogError($"GameFlowController: Panel for state {newState} not found!");
            }
        }
    }
}
