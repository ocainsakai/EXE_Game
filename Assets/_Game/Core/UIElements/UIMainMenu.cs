using _Game.Core.Gameplay;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    private ISceneLoader SceneLoader => global::_Game.Core.Gameplay.SceneLoader.Instance;

    public void VaoTran()
    {
        SceneLoader.LoadSceneName("Map");
    }
    public void OpenDeck()
    {
        
    }

    public void OpenCollection()
    {
        
    }

}
