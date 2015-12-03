using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class FinishPanelScript : MonoBehaviour {
    void Update()
    {
        string typecourse="";
        GameObject gameControllerObject = GameObject.FindWithTag("gameController");
        if (gameControllerObject != null)
        {
            GameController g = gameControllerObject.GetComponent<GameController>();
            typecourse = g.typeCourse;
        }
        if(typecourse== "Contre la montre")
        {
            Text classement = GameObject.Find("Classement").GetComponent<Text>();
            float tempsfinal = GameObject.Find("Airship(Clone)").GetComponent<PlayerController>().tempsfinal;
            int minute = 0;
            while (tempsfinal > 60)
            {
                minute++;
                tempsfinal = tempsfinal - 60;
            }
            classement.text = "Votre temps final est: " + minute.ToString() + ":" + tempsfinal.ToString("00.000");
        }

    }
    public void Retry()
    {
        string element = "";
        string typecourse = "";
        GameObject gameControllerObject = GameObject.FindWithTag("gameController");
        if (gameControllerObject != null)
        {
            GameController g = gameControllerObject.GetComponent<GameController>();
            typecourse = g.typeCourse;
            element = g.element;
            g.clearGameController();
            if (typecourse == "Contre la montre")
            {
                g.typeCourse = typecourse;
                g.element = element;
                GameObject ui = GameObject.Find("UI");
                ui.GetComponent<StartOptions>().setMapLoad(Application.loadedLevel);
                ui.GetComponent<StartOptions>().StartButtonClicked();
            }
            else
                Application.LoadLevel(Application.loadedLevel);
        }
    }

    public void ToMainMenu()
    {
        GameObject canvas = GameObject.FindGameObjectWithTag("Canvas");
        canvas.SetActive(false);

        GameObject gameControllerObject = GameObject.FindWithTag("gameController");
        if (gameControllerObject != null)
        {
            GameController g = gameControllerObject.GetComponent<GameController>();
            g.clearGameController();
        }

        GameObject ui = GameObject.Find("UI");

        ui.GetComponent<ShowPanels>().ShowBackground();
        ui.GetComponent<ShowPanels>().ShowMenuMain();
    }

}
