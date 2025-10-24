using BulletHellTemplate;
using UnityEngine;

namespace _Game.Core.UIElements
{
    public class UIScreens : MonoBehaviour
    {

        [SerializeField] private GameObject winScreens;
        [SerializeField] private GameObject completeScreens;
        [SerializeField] private GameObject loseScreens;
        [SerializeField] private GameObject pauseScreens;

        [SerializeField] private AudioClip winVfx;
        [SerializeField] private AudioClip loseVfx;

        private void CloseAllScreens()
        {
            winScreens.SetActive(false);    
            completeScreens.SetActive(false);
            loseScreens.SetActive(false);
            pauseScreens.SetActive(false);
        }
        
        public void ShowWinScreens()
        {
            CloseAllScreens();
            AudioManager.Singleton.PlayAudio(winVfx, "master");
            winScreens.SetActive(true);
        }
        public void ShowLoseScreens()
        {
            CloseAllScreens();
            loseScreens.SetActive(true);
            AudioManager.Singleton.PlayAudio(loseVfx, "master");
        }

        public void ShowCompleteScreens()
        {
            CloseAllScreens();
            completeScreens.SetActive(true);
            AudioManager.Singleton.PlayAudio(winVfx, "master");
            
        }
        public void ShowPauseScreens()
        {
            CloseAllScreens();
            pauseScreens.SetActive(true);
        }
    }
}
