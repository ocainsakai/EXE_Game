using System;
using UnityEngine;
using UnityEngine.UI;

namespace Test
{
    public class GameController : MonoBehaviour
    {
        // data
        public int[] mapSaved;
        public int playerPos;
        public bool battleResult;

        // event
        public Button battleStart;
        public Action<int> startBattle;
        public Action<bool> endBattleAndResult;
        public UIManager manager;

        public void Awake()
        {
            manager.CloseAll();

        }
        public void Start()
        {
            
        }
        public void BattleRequest(int[] mapSaved, int playerPos)
        {
            this.mapSaved = mapSaved;
            battleStart.interactable = true;
            //LoadBattleScene();
        }
        public void LoadBattleScene()
        {
            Debug.Log("battle loading ...");
            manager.CloseAll();
            startBattle?.Invoke(mapSaved[playerPos+1]);
        }
        public void LoadMapScene()
        {
            Debug.Log("map loading ...");
            manager.CloseAll();
            endBattleAndResult?.Invoke(battleResult);
        }
        public void Clamp()
        {
            Debug.Log($"You get {mapSaved[playerPos+1]}");
            LoadMapScene();
        }
    }
}