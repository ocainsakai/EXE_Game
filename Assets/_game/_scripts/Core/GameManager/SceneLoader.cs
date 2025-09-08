using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public interface ISceneLoader
{
    SceneLoadRequest LoadScene(string sceneName);
    void ReloadCurrentScene(Action onLoaded = null);
    ITransitionDataService GetTransitionDataService();
}
public class SceneLoader : MonoBehaviour, ISceneLoader
{
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private float minLoadingTime = 0.5f;

    private ITransitionDataService transitionDataService;

    private void Awake()
    {
        transitionDataService = new TransitionDataService();
    }

    public SceneLoadRequest LoadScene(string sceneName)
    {
        return new SceneLoadRequest(sceneName, this);
    }

    internal void LoadScene(string sceneName, object data = null, Action onLoaded = null)
    {
        StartCoroutine(LoadSceneAsync(sceneName, data, onLoaded));
    }

    public void ReloadCurrentScene(Action onLoaded = null)
    {
        string currentScene = SceneManager.GetActiveScene().name;
        LoadScene(currentScene, null, onLoaded);
    }

    private IEnumerator LoadSceneAsync(string sceneName, object data = null, Action onLoaded = null)
    {
        if (data != null)
            transitionDataService.SetData(data);

        if (loadingScreen != null)
            loadingScreen.SetActive(true);

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;

        float elapsed = 0f;

        while (op.progress < 0.9f) yield return null;
        while (elapsed < minLoadingTime) { elapsed += Time.deltaTime; yield return null; }

        op.allowSceneActivation = true;

        while (!op.isDone) yield return null;

        if (loadingScreen != null)
            loadingScreen.SetActive(false);

        onLoaded?.Invoke();
    }

    public ITransitionDataService GetTransitionDataService() => transitionDataService;
}

