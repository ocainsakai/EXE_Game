using UnityEngine;

namespace _Game.Core.UIElements
{
    public class UIScreens : MonoBehaviour
    {

        [SerializeField] private GameObject winScreens;
        [SerializeField] private GameObject loseScreens;
        [SerializeField] private GameObject pauseScreens;

        public void ShowWinScreens()
        {
            winScreens.SetActive(true);
        }
        public void ShowLoseScreens()
        {
            loseScreens.SetActive(true);
        }
        public void ShowPauseScreens()
        {
            pauseScreens.SetActive(true);
        }
    }
}
