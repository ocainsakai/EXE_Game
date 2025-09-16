using TMPro;
using UnityEngine;

public class TextDisplay : MonoBehaviour
{
    public TextMeshProUGUI textMesh;

    public void UpdateContent(string content)
    {
        textMesh.text = content;
    }
}
