using UnityEngine;
using UnityEngine.UI;

public class ButtonClose : MonoBehaviour
{
    [SerializeField] GameObject panelToClose;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(() =>
            {
                panelToClose.SetActive(false);
            });
        }
    }
}
