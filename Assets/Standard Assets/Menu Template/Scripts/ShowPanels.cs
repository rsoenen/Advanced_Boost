using UnityEngine;
using System.Collections;

public class ShowPanels : MonoBehaviour {

	public GameObject optionsPanel;							//Store a reference to the Game Object OptionsPanel 
	public GameObject optionsTint;							//Store a reference to the Game Object OptionsTint 
	public GameObject menuPanel;							//Store a reference to the Game Object MenuPanel 
	public GameObject pausePanel;							//Store a reference to the Game Object PausePanel 
	public GameObject menuSolo;
	public GameObject menuChoixCircuit;
	public GameObject menuWithCoopOrNot;

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
	public void ShowMenu()
	{
        HideMenuSolo();
        HideMenuChoixCircuit();
        HideMenuWithCoopOrNot();

		menuPanel.SetActive (true);
	}
	//Call this function to deactivate and hide the main menu panel during the main menu
	public void HideMenu()
	{
		menuPanel.SetActive (false);
	}

	//Call this function to activate and display the main menu with Coop or not during the main menu
	public void ShowMenuWithCoopOrNot()
	{
        HideMenu();
        HideMenuSolo();
        HideMenuChoixCircuit();

		menuWithCoopOrNot.SetActive (true);
	}
	//Call this function to deactivate and hide the main menu with Coop or not during the main menu
	public void HideMenuWithCoopOrNot()
	{
		menuWithCoopOrNot.SetActive (false);
	}

	//Call this function to activate and display the solo menu panel during the main menu
	public void ShowMenuSolo()
	{
        HideMenu();
        HideMenuChoixCircuit();
        HideMenuWithCoopOrNot();

		menuSolo.SetActive (true);
	}
	//Call this function to deactivate and hide the main menu solo during the main menu
	public void HideMenuSolo()
	{
		menuSolo.SetActive (false);
	}

	//Call this function to activate and display the main menu choix circuit during the main menu
	public void ShowMenuChoixCircuit()
	{
        HideMenu();
        HideMenuSolo();
        HideMenuWithCoopOrNot();

		menuChoixCircuit.SetActive (true);
	}
	//Call this function to deactivate and hide the main menu choix circuit during the main menu
	public void HideMenuChoixCircuit()
	{
		menuChoixCircuit.SetActive (false);
	}
	
	
}
