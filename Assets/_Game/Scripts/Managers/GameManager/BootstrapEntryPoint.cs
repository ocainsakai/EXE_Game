using UnityEngine;
using VContainer;
using VContainer.Unity;

public class BootstrapEntryPoint : IStartable
{
    private ISceneLoader sceneLoader => SceneLoader.Instance;
    public void Start()
    {
        Debug.Log("bootrap exe");
        sceneLoader.LoadSceneName("MainMenu").Execute();
    }

}
