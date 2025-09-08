using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class BootstrapEntryPoint : IStartable
{
    private readonly ISceneLoader sceneLoader;

    [Inject]
    public BootstrapEntryPoint(ISceneLoader sceneLoader)
    {
        this.sceneLoader = sceneLoader;
    }

    public void Start()
    {
        sceneLoader.LoadScene("MainMenu");

    }

}
