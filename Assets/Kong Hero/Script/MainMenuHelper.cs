using UnityEngine;
public class MainMenuHelper : MonoBehaviour {
	public GameObject Helper;
	public GameObject OptionPanel;
	public GameObject TutorialPanel;
	private GameObject _currentPanel;
	// Use this for initialization
	void Start () {
		HideHelper();
		_currentPanel = OptionPanel;
		ShowOption();
	}

	public void ShowHelper(){
		Helper.SetActive (true);
	}
	public void Toggle()
	{
		if (_currentPanel == OptionPanel)
		{
			_currentPanel = TutorialPanel;
		}
		else
		{
			_currentPanel = OptionPanel;
		}
        UnactiveAll();
        _currentPanel.transform.Find("hover").gameObject.SetActive(true);
        _currentPanel.transform.Find("holder").gameObject.SetActive(true);
    }
    public void HideHelper(){
		Helper.SetActive (false);
	}
	public void ShowOption()
	{
		UnactiveAll();
		OptionPanel.transform.Find("hover").gameObject.SetActive(true);
		OptionPanel.transform.Find("holder").gameObject.SetActive(true);
	}
	public void ShowTutorial()
	{
		UnactiveAll();
        TutorialPanel.transform.Find("hover").gameObject.SetActive(true);
        TutorialPanel.transform.Find("holder").gameObject.SetActive(true);
    }
	private void UnactiveAll()
	{
        TutorialPanel.transform.Find("hover").gameObject.SetActive(false);
        TutorialPanel.transform.Find("holder").gameObject.SetActive(false);
        OptionPanel.transform.Find("hover").gameObject.SetActive(false);
        OptionPanel.transform.Find("holder").gameObject.SetActive(false);
    }
}
