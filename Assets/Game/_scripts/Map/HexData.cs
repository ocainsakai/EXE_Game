using UnityEngine;
using UnityEngine.Tilemaps;

namespace Map
{
    [CreateAssetMenu(fileName = "HexData", menuName = "Scriptable Objects/HexData")]
    public class HexData : ScriptableObject
    {
        public string Name;
        public HexType Type;
        public Tile Tile;
    }
}