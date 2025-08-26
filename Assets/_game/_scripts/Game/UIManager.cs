using UnityEngine;

public class UIManager : ManualSingleton<UIManager>
{
    [SerializeField] GameObject winUI;
    [SerializeField] GameObject loseUI;
    [SerializeField] GameObject pauseUI;
    
    private void CloseAll()
    {
        winUI?.SetActive(false);
        loseUI?.SetActive(false);
        pauseUI?.SetActive(false);
    }

    public void OnWin()
    {
        CloseAll();
        winUI?.SetActive(true);
    }
    public void OnLose()
    {
        CloseAll();
        loseUI?.SetActive(true);
    }
    public void OnPause()
    {
        CloseAll();
        pauseUI?.SetActive(true);
    }
}
