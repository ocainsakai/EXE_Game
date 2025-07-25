using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject startPanel;
    public GameObject continuePanel;

    void Start()
    {
        //PlayerPrefs.DeleteAll(); --Test StartPanel--
        //PlayerPrefs.SetInt("HasStarted", 1); --Test ContinuePanel--
        // Check if player has started before
        bool hasStarted = PlayerPrefs.GetInt("HasStarted", 0) == 1;

        // Show correct panel
        startPanel.SetActive(!hasStarted);
        continuePanel.SetActive(hasStarted);
    }

    public void StartGame()
    {
        PlayerPrefs.SetInt("HasStarted", 1);
        SceneManager.LoadScene("LoadingScene");
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene("LoadingScene");
    }

    public void NewGame()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("LoadingScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
