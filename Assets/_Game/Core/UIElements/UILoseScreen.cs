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
        // Logic của Lose: Xem Ad để... (về menu? hoặc hồi sinh?)
        // Hiện tại code của bạn đang là về Main Menu
        Debug.Log("Watching Ad (from lose screen)...");
        
        AdManager.Instance.ShowRewardedAd(() =>
        {
            // Logic sau khi xem ad (ví dụ: hồi sinh)
            // Tạm thời theo code cũ của bạn là về Main Menu
            SceneLoader.Instance.LoadScene("MainMenu");
            Hide();
        });
    }
}
