using _Game.Core.Gameplay;
using UnityEngine;

public class UILoseScreen : BaseScreen
{
    protected override void OnButton1Clicked()
    {
        // Logic của Lose: Về Main Menu
        Debug.Log("Returning to Main Menu...");
        SceneLoader.Instance.LoadScene("MainMenu");
        Hide();
    }

    protected override void OnButton2Clicked()
    {
        Debug.Log("Watching Ad (from lose screen)...");
        
        AdsManager.Instance.ShowRewardedAd(() =>
        {
            SceneLoader.Instance.LoadScene("MainMenu");
            Hide();
        });
    }
}
