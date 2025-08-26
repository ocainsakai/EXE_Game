using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Map
{
    public class MapManager : MonoBehaviour
    {
        [Header("Internal")]
        [SerializeField] MapGenerator mapGenerator;
        [SerializeField] MapUI uiManager;

        [Header("Data")]
        [SerializeField]
        //private EnemyCollection enemyDatas;
        //[SerializeField]
        //private HexesRuntimeCollection mapState;

        public List<HexRuntime> hexStates = new List<HexRuntime>();
        public Vector2Int playerPosition { get; private set; } = new Vector2Int(0, 0);
        private Vector2Int lastPosition = new Vector2Int(0, 0);
        private HexRuntime lastHex;
        private void Awake()
        {
            HexController.OnHexClicked += OnHexClicked;
            PopupUI.onClick += OnMoveToHex;
        }

        private void OnMoveToHex()
        {
            // save data
            //GameManager.Instance.battleContext = new BattleContext(enemyDatas.FirstOrDefault());
            GameManager.Instance.ChangeScenceToCombat();
        }

        private void OnHexClicked(HexRuntime runtime)
        {
            lastHex = runtime;
            lastPosition = runtime.position;
            bool value = playerPosition.HasRight(lastPosition);
            uiManager.OpenPopupUI(runtime,value);
        }
    }
}
