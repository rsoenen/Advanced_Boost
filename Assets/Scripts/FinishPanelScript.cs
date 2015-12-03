using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class FinishPanelScript : MonoBehaviour {
    public bool lauching = false;
    void Update()
    {
        string typecourse="";
        int CourseActuelDuChampionnat = 1;
        GameObject gameControllerObject = GameObject.FindWithTag("gameController");
        if (gameControllerObject != null)
        {
            GameController g = gameControllerObject.GetComponent<GameController>();
            typecourse = g.typeCourse;
            CourseActuelDuChampionnat = g.CourseActuelDuChampionnat;
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
        else if (typecourse == "Championnat" && !lauching)
        {
            if (CourseActuelDuChampionnat < 4)
                GameObject.Find("RetryText").GetComponent<Text>().text = "Continuer";
            else
                GameObject.Find("RetryText").GetComponent<Text>().text = "Recommencer le Championnat";
        }

    }
    public void Retry()
    {
        string element = "";
        string typecourse = "";
        int numeroCourse= 1;
        int [] PointsSave = new int[8];
        GameObject gameControllerObject = GameObject.FindWithTag("gameController");
        if (gameControllerObject != null)
        {
            GameController g = gameControllerObject.GetComponent<GameController>();
            typecourse = g.typeCourse;
            element = g.element;
            numeroCourse = g.CourseActuelDuChampionnat;
            if (g.typeCourse == "Championnat" && g.CourseActuelDuChampionnat < 4)
            {
                for(int i=0; i<8;i++)
                {
                    PointsSave[i] = g.PointDuChampionnat[i];
                }
            }

            g.clearGameController();
            if (typecourse == "Contre la montre")
            {
                g.typeCourse = typecourse;
                g.element = element;
                GameObject ui = GameObject.Find("UI");
                ui.GetComponent<StartOptions>().setMapLoad(Application.loadedLevel);
                ui.GetComponent<StartOptions>().StartButtonClicked();
            }
            else if (typecourse == "Championnat")
            {
                g.typeCourse = typecourse;
                g.element = element;
                g.CourseActuelDuChampionnat = numeroCourse;
                if (g.CourseActuelDuChampionnat < 4)
                {
                    for(int i=0; i<8;i++)
                    {
                        g.PointDuChampionnat[i] = PointsSave[i];
                    }
                    GameObject ui = GameObject.Find("UI");
                    lauching = true;
                    Application.LoadLevel(Application.loadedLevel + 1);
                    g.CourseActuelDuChampionnat++;
                }
                else
                {
                    g.clearGameController();
                    g.typeCourse = typecourse;
                    g.element = element;
                    GameObject ui = GameObject.Find("UI");
                    Application.LoadLevel(Application.loadedLevel -3);
                    ui.GetComponent<StartOptions>().StartButtonClicked();
                }
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
