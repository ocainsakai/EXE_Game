using UnityEngine;

public class UIGameplay : MonoBehaviour
{
    [SerializeField] GameObject uiMapOpen;
    [SerializeField] GameObject uiDeckOpen;
    [SerializeField] GameObject uiPlayerAction;
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
        uiPlayerAction.SetActive(false);
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
        uiPlayerAction.SetActive(true);
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
        uiPlayerAction.SetActive(true);
        uiBattle.SetActive(true);
        uiBattle.GetComponent<UIBattle>().Show(enemyData);
    }
    public void OnBattleWin()
    {
        uiPlayerAction.gameObject.SetActive(false);
        uiMapOpen.gameObject.SetActive(true);
        uiBattle.gameObject.SetActive(false);
    }  
}
