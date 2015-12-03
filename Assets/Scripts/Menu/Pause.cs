using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Pause : MonoBehaviour {

	private ShowPanels showPanels;						//Reference to the ShowPanels script used to hide and show UI panels
	private bool isPaused;								//Boolean to check if the game is paused or not
	private StartOptions startScript;					//Reference to the StartButton script
	
	//Awake is called before Start()
	void Awake()
	{
		//Get a component reference to ShowPanels attached to this object, store in showPanels variable
		showPanels = GetComponent<ShowPanels> ();
		//Get a component reference to StartButton attached to this object, store in startScript variable
		startScript = GetComponent<StartOptions> ();
        
	}

	// Update is called once per frame
	void Update () {

        string typecourse = "";
        GameObject gameControllerObject = GameObject.FindWithTag("gameController");
        if (gameControllerObject != null)
        {
            GameController g = gameControllerObject.GetComponent<GameController>();
            typecourse = g.typeCourse;
        }
        //Check if the Cancel button in Input Manager is down this frame (default is Escape key) and that game is not paused, and that we're not in main menu
        if (Input.GetButtonDown ("Cancel") && !isPaused && !startScript.inMainMenu)
        {
            if (typecourse == "Contre la montre")
            {
                if(GameObject.Find("Airship(Clone)").GetComponent<PlayerController>().isPlayerRunning)
                    DoPause();
            }
            else 
            {
                if (GameObject.FindWithTag("Player1").GetComponent<PlayerController>().isPlayerRunning)
                    DoPause();
            }

        }
		//If the button is pressed and the game is paused and not in main menu
		else if (Input.GetButtonDown ("Cancel") && isPaused && !startScript.inMainMenu) 
		{
			//Call the UnPause function to unpause the game
			UnPause ();
		}
	
	}


	public void DoPause()
	{
		//Set isPaused to true
		isPaused = true;
		//Set time.timescale to 0, this will cause animations and physics to stop updating
		Time.timeScale = 0;
		//call the ShowPausePanel function of the ShowPanels script
		showPanels.ShowPausePanel ();
	}


	public void UnPause()
	{
		//Set isPaused to false
		isPaused = false;
		//Set time.timescale to 1, this will cause animations and physics to continue updating at regular speed
		Time.timeScale = 1;
        //call the HidePausePanel function of the ShowPanels script
        showPanels.HidePausePanel();
	}

    public void Retry()
    {
        string element = "";
        string typecourse = "";
        UnPause();

        GameObject gameControllerObject = GameObject.FindWithTag("gameController");
        if (gameControllerObject != null)
        {
            GameController g = gameControllerObject.GetComponent<GameController>();
            typecourse = g.typeCourse;
            element = g.element;
            if (typecourse == "Contre la montre")
            {
                g.clearGameController();
                g.typeCourse = typecourse;
                g.element = element;
                GameObject ui = GameObject.Find("UI");
                ui.GetComponent<StartOptions>().setMapLoad(Application.loadedLevel);
                ui.GetComponent<StartOptions>().StartButtonClicked();
            }
            else if (typecourse == "Championnat")
            {

                g.clearGameController();
                g.typeCourse = typecourse;
                g.element = element;
                GameObject ui = GameObject.Find("UI");
                ui.GetComponent<StartOptions>().setMapLoad(Application.loadedLevel - g.CourseActuelDuChampionnat+1);
                ui.GetComponent<StartOptions>().StartButtonClicked();
            }
            else
            {
                g.clearGameController();
                Application.LoadLevel(Application.loadedLevel);
            }
        }
    }

    public void Options()
    {
        showPanels.HidePausePanel();
        showPanels.ShowOptionsPanel();
    }

    public void ToMainMenu()
    {
        UnPause();

        GameObject canvas = GameObject.FindGameObjectWithTag("Canvas");
        canvas.SetActive(false);  

        GameObject gameControllerObject = GameObject.FindWithTag("gameController");
        if (gameControllerObject != null){
            GameController g = gameControllerObject.GetComponent<GameController>();
            g.clearGameController();
        }

        

        showPanels.ShowBackground();
        showPanels.ShowMenuMain();
    }
}
