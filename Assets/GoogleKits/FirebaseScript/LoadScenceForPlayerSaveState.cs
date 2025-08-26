using Cysharp.Threading.Tasks;
using UnityEngine;

public class LoadScenceForPlayerSaveState : MonoBehaviour
{
    [SerializeField] private PlayerSaveManager _playerSaveManager;
    [SerializeField] private string _sceneForPlayerSaveState;
    [SerializeField] private string _sceneForPlayerNoSaveState;
    
    public void Trigger()
    {
        _ = LoadSceneAsync();
    }
    private async UniTask LoadSceneAsync()
    {
        var saveExists = await _playerSaveManager.SaveExists();

        if (saveExists)
        {
            await UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(_sceneForPlayerSaveState);
        }
        else
        {
            Debug.Log("No player save found, loading scene for player no save state.");
            await UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(_sceneForPlayerNoSaveState);
        }
    }
}
