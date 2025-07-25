using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoScreenController : MonoBehaviour
{
    void Start()
    {
        Invoke("LoadNextScene", 2f); // Wait 2 seconds
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}
