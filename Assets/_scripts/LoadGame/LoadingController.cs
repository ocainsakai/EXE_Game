using UnityEngine;

public class LoadingController : MonoBehaviour
{
    void Start()
    {
        Invoke("LoadGameScene", 2f); // simulate loading
    }

    void LoadGameScene()
    {
        Debug.Log("Game would load here.");
        // You can add SceneManager.LoadScene("GameScene") later
    }
}
