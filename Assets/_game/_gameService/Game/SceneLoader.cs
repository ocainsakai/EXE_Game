using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public interface ISceneLoader
{
    void LoadScene(string sceneName, Action onLoaded = null);
    void ReloadCurrentScene(Action onLoaded = null);
    IEnumerator LoadSceneAsync(string sceneName, Action onLoaded = null);
}


public class SceneLoader : MonoBehaviour, ISceneLoader
{
    [SerializeField] private GameObject loadingScreen;


    public void LoadScene(string sceneName, Action onLoaded = null)
    {
        StartCoroutine(LoadSceneAsync(sceneName, onLoaded));
    }

    public void ReloadCurrentScene(Action onLoaded = null)
    {
        LoadScene(SceneManager.GetActiveScene().name, onLoaded);
    }
    public IEnumerator LoadSceneAsync(string sceneName, Action onLoaded = null)
    {
        if (loadingScreen != null) loadingScreen.SetActive(true);

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;

        while (op.progress < 0.9f) yield return null;

        yield return new WaitForSeconds(0.5f); // cho animation hoặc fade

        op.allowSceneActivation = true;
        while (!op.isDone) yield return null;

        if (loadingScreen != null) loadingScreen.SetActive(false);
        onLoaded?.Invoke();
    }
}
