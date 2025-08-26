using UnityEngine;

namespace Map
{
    public class HexDataHolder : MonoBehaviour
    {
        public HexRuntime Data { get; private set; }
        public void SetData(HexRuntime data)
        {
            Data = data;
        }
    }
}