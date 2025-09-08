using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class BootstrapEntryPoint : IStartable
{
    private readonly ISceneLoader sceneLoader;
    private readonly UILoading loadingUI;

    [Inject]
    public BootstrapEntryPoint(ISceneLoader sceneLoader, UILoading loadingUI)
    {
        this.sceneLoader = sceneLoader;
        this.loadingUI = loadingUI;
    }

    public void Start()
    {
        RunBootstrapFlow();
    }

    private async void RunBootstrapFlow()
    {
        loadingUI.Show();

        var startTime = Time.time;

        // 🛠️ Giả lập init service
        await UniTask.Delay(200); // Config
        loadingUI.SetProgress(0.3f);

        await UniTask.Delay(200); // Audio
        loadingUI.SetProgress(0.6f);

        await UniTask.Delay(200); // Save/Network
        loadingUI.SetProgress(1f);

        // đảm bảo loading screen tối thiểu 0.5s
        var elapsed = Time.time - startTime;
        if (elapsed < 0.5f)
        {
            await UniTask.Delay(System.TimeSpan.FromSeconds(0.5f - elapsed));
        }

        // Chuyển sang MainMenu
        sceneLoader.LoadScene("MainMenu");
        loadingUI.Hide();
    }
}
