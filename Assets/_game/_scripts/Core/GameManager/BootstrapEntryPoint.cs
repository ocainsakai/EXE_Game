using UnityEngine;
using VContainer;
using VContainer.Unity;

public class BootstrapEntryPoint : IStartable
{
    private readonly ISceneLoader sceneLoader;

    [Inject]
    public BootstrapEntryPoint(ISceneLoader sceneLoader)
    {
        Debug.Log(sceneLoader);
        this.sceneLoader = sceneLoader;
    }

    public void Start()
    {
        Debug.Log("bootrap exe");
        sceneLoader.LoadSceneName("MainMenu").Execute();
    }

}
