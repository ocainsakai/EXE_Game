using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityUtils;

namespace _Game.Core.Gameplay
{
    public interface ISceneLoader
    {
        SceneLoadRequest LoadSceneName(string sceneName);
        void ReloadCurrentScene(Action onLoaded = null);
    }
    public class SceneLoader : Singleton<SceneLoader>, ISceneLoader
    {
        [SerializeField] private GameObject loadingScreen;
        [SerializeField] private float minLoadingTime = 0.5f;

        public SceneLoadRequest LoadSceneName(string sceneName)
        {
            Debug.Log("on loading...");  
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
        
            if (loadingScreen is not null && loadingScreen)
                loadingScreen.SetActive(true);

            AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
            if (op != null)
            {
                op.allowSceneActivation = false;

                var elapsed = 0f;

                while (op.progress < 0.9f) yield return null;
                while (elapsed < minLoadingTime)
                {
                    elapsed += Time.deltaTime;
                    yield return null;
                }

                op.allowSceneActivation = true;

                while (!op.isDone) yield return null;
            }

            loadingScreen?.SetActive(false);

            onLoaded?.Invoke();
        }

        protected override void Awake()
        {
            base.Awake();
            if (Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
        }
        void Start()
        {
            if (loadingScreen != null)
                loadingScreen.SetActive(false);
            if (SceneManager.GetActiveScene().name == "Entry")
            {
                LoadScene("MainMenu");  
            }
        }
    }
}