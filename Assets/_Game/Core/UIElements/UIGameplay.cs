using UnityEngine;

public class UIGameplay : MonoBehaviour
{
    [SerializeField] GameObject uiMapOpen;
    [SerializeField] GameObject uiDeckOpen;
    [SerializeField] GameObject uiMap;
    [SerializeField] GameObject uiBattle;

    void Start()
    {
        ShowOpener();
    }
    public void HideAll()
    {
        uiMapOpen.SetActive(false);
        uiDeckOpen.SetActive(false);
        uiMap.SetActive(false);
        uiBattle.SetActive(false);
    }
    public void ShowOpener()
    {
        HideAll();
        uiDeckOpen.SetActive(true);
        uiMapOpen.SetActive(true);
    }
    public void ShowPlayerAction()
    {
        HideAll();
    }
   
    public void ShowMap()
    {
        HideAll();
        uiMap.SetActive(true);
    }
    public void ShowBattle(EnemyData enemyData)
    {
        HideAll();
        uiDeckOpen.SetActive(true);
        uiBattle.SetActive(true);
    }
    public void OnBattleWin()
    {
        uiMapOpen.gameObject.SetActive(true);
        uiBattle.gameObject.SetActive(false);
    }  
}
