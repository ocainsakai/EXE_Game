using UnityEngine;
using UnityEngine.UI;

public class UILoading : MonoBehaviour
{
    [SerializeField] private GameObject root;
    [SerializeField] private Slider progressBar;

    public void Show()
    {
        root.SetActive(true);
        if (progressBar != null) progressBar.value = 0;
    }

    public void Hide()
    {
        if (root != null) root.SetActive(false);
    }

    public void SetProgress(float value)
    {
        if (progressBar != null)
            progressBar.value = Mathf.Clamp01(value);
    }
}
