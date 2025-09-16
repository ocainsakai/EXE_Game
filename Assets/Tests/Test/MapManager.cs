using UnityEngine;
using UnityEngine.UI;

namespace Test
{
    public class MapManager : MonoBehaviour
    {
        public GameController controller;
        public int playerPosition = 0;
        public int[] mapState = new int[10];
        public Button create;
        public Button move;
        //public Act
        public void Start()
        {
            create.interactable = true;
        }
        public void GenerateMap()
        {
            move.interactable = true;
            for (int i = 0; i < mapState.Length; i++)
            {
                mapState[i] = Random.Range(2,10);
            }
            mapState[0] = 1;
            mapState[9] = 9;
        }
        public void MoveNext()
        {
            playerPosition++;
            if (mapState[playerPosition] > 3)
            {
                controller.BattleRequest(mapState, playerPosition);
            }
            else
            {
                mapState[playerPosition] = 1;
                mapState[playerPosition - 1] = 0;
            }

        }
    }
}