using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShowPanels : MonoBehaviour {

	public GameObject optionsPanel;							//Store a reference to the Game Object OptionsPanel 
	public GameObject optionsTint;							//Store a reference to the Game Object OptionsTint 
	public GameObject pausePanel;							//Store a reference to the Game Object PausePanel 
    public GameObject menuMain;	
    public GameObject menuSolo;
	public GameObject menuSelectCourse;
    public GameObject menuSelectCourse1;
	public GameObject menuMulti;
    public GameObject menuChoixElement;
    public GameObject menuChoixChampionnat;

    public GameObject background;

	//Call this function to activate and display the Options panel during the main menu
	public void ShowOptionsPanel()
	{
		optionsPanel.SetActive(true);
		optionsTint.SetActive(true);
	}
	//Call this function to deactivate and hide the Options panel during the main menu
	public void HideOptionsPanel()
	{
		optionsPanel.SetActive(false);
		optionsTint.SetActive(false);
	}

    //Call this function to activate and display the Pause panel during game play
    public void ShowPausePanel()
    {
        pausePanel.SetActive(true);
        optionsTint.SetActive(true);
    }
    //Call this function to deactivate and hide the Pause panel during game play
    public void HidePausePanel()
    {
        pausePanel.SetActive(false);
        optionsTint.SetActive(false);
    }

	//Call this function to activate and display the main menu panel during the main menu
	public void ShowMenuMain()
	{
		menuMain.SetActive (true);
	}
	//Call this function to deactivate and hide the main menu panel during the main menu
	public void HideMenuMain()
	{
		menuMain.SetActive (false);
	}

	//Call this function to activate and display the main menu with Coop or not during the main menu
	public void ShowMenuMulti()
	{
		menuMulti.SetActive (true);
	}
	//Call this function to deactivate and hide the main menu with Coop or not during the main menu
	public void HideMenuMulti()
	{
		menuMulti.SetActive (false);
	}

	//Call this function to activate and display the solo menu panel during the main menu
	public void ShowMenuSolo()
	{
		menuSolo.SetActive (true);
	}
	//Call this function to deactivate and hide the main menu solo during the main menu
	public void HideMenuSolo()
	{
		menuSolo.SetActive (false);
	}

	//Call this function to activate and display the main menu choix circuit during the main menu
	public void ShowMenuSelectCourse()
	{
		menuSelectCourse.SetActive (true);
	}
	//Call this function to deactivate and hide the main menu choix circuit during the main menu
	public void HideMenuSelectCourse()
	{
		menuSelectCourse.SetActive (false);
	}

    //Call this function to activate and display the main menu choix circuit 1 during the main menu
    public void ShowMenuSelectCourse1()
    {
        menuSelectCourse1.SetActive(true);
    }
    //Call this function to deactivate and hide the main menu choix circuit 1 during the main menu
    public void HideMenuSelectCourse1()
    {
        menuSelectCourse1.SetActive(false);
    }

    //Call this function to deactivate and hide the main menu choix elements during the main menu
    public void ShowMenuChoixElement()
    {
        menuChoixElement.SetActive(true);
    }
    public void HideMenuChoixElement()
    {
        menuChoixElement.SetActive(false);
    }
    //Call this function to deactivate and hide the main menu choix championnat during the main menu
    public void ShowMenuChoixChampionnat()
    {
        menuChoixChampionnat.SetActive(true);
    }
    public void HideMenuChoixChampionnat()
    {
        menuChoixChampionnat.SetActive(false);
    }

    public void ShowBackground()
    {
        background.SetActive(true);
    }
    public void HideBackground()
    {
        background.SetActive(false);
    }
}
