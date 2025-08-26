
using UnityEditor;
using UnityEngine;
[CustomPropertyDrawer(typeof(DataAsset))]
public class DataAssetDrawer : PropertyDrawIterator
{
    public DataAssetDrawer(Rect rect, SerializedProperty property, bool drawLabel) : base(rect, property, drawLabel)
    {
    }

}
