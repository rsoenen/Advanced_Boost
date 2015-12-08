using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class FinishPanelScript : MonoBehaviour {
    public bool lauching = false;
    private bool IsSavingRecord = true;

    void Update()
    {

        string typeCourse = "";
        int currentTrack = 1;
        GameObject gameControllerObject = GameObject.FindWithTag("gameController");

        if (gameControllerObject != null)
        {
            GameController g = gameControllerObject.GetComponent<GameController>();
            typeCourse = g.typeCourse;
            currentTrack = g.currentTrack;
        }
        bool save = GameObject.Find("Airship(Clone)").GetComponent<PlayerController>().isPlayerRunning;

        if (typeCourse == "Contre la montre" && !save && IsSavingRecord)
        {
            Text classement = GameObject.Find("Classement").GetComponent<Text>();
            float tempsfinal = GameObject.Find("Airship(Clone)").GetComponent<PlayerController>().tempsfinal;

            int minutes = (int)(tempsfinal) / 60;
            float seconds = tempsfinal % 60;
            

            classement.text = "Votre temps final est: " + minutes.ToString() + ":" + seconds.ToString("00.000") + "\n";
            if (tempsfinal <= getSavedTime(currentTrack) )
            {
                classement.text += "Nouveau record ! \n";
                saveNewTime(currentTrack, tempsfinal);
                //IsSavingRecord = false;
            }
            else
            {
                float timeRecord = getSavedTime(currentTrack);
                int minutesRecord = (int)(timeRecord)/60;
                float secondsRecord = timeRecord%60;
                classement.text += "Votre record est de : " + minutesRecord.ToString() + ":" + secondsRecord.ToString("00.000") + "\n";
            }

        }
        else if (typeCourse == "Championnat" && !lauching)
        {
            if (currentTrack < 4)
                GameObject.Find("RetryText").GetComponent<Text>().text = "Continuer";
            else
                GameObject.Find("RetryText").GetComponent<Text>().text = "Recommencer";
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
            numeroCourse = g.currentTrack;
            if (g.typeCourse == "Championnat" && g.currentTrack < 4)
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
                g.currentTrack = numeroCourse;
                if (g.currentTrack < 4)
                {
                    for(int i=0; i<8;i++)
                    {
                        g.PointDuChampionnat[i] = PointsSave[i];
                    }
                    GameObject ui = GameObject.Find("UI");
                    lauching = true;
                    Application.LoadLevel(Application.loadedLevel + 1);
                    g.currentTrack++;
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

        ui.GetComponent<PlayMusic>().PlaySelectedMusic(0);
        ui.GetComponent<ShowPanels>().ShowBackground();
        ui.GetComponent<ShowPanels>().ShowMenuMain();
        
    }

    private float getSavedTime(int track)
    {
        return PlayerPrefs.GetFloat("CLM_Track" + track, 1000000);
    }

    private void saveNewTime(int track, float time)
    {
        PlayerPrefs.SetFloat("CLM_Track" + track, time);
    }
}
