using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SecondPlayerController : VehicleController{

    #region Variables

    public Text lapText;
    public Text timeElapsedText;

    private GameObject BonneConduite;
    private GameObject SpeedBar;
    private GameObject TurboBar;

    int minute;

    #endregion

    void Start()
    {
        BonneConduite = GameObject.FindWithTag("BonneConduite");
        SpeedBar=GameObject.FindWithTag("Vitesse");
        TurboBar=GameObject.FindWithTag("Turbo");
    }

    void FixedUpdate()
    {
        #region AffichageTimeElapsed
        minute = 0;
        float time = Time.time;
        while(time>60)
        {
            minute++;
            time = time - 60;
        }
        if(minute>0)
            timeElapsedText.text = "Time: " + minute.ToString() + ":" + time.ToString("00.000");
        else
            timeElapsedText.text = "Time: " + time.ToString("0:00.000");
        #endregion
        #region AffichageCompteurTour
        lapText.text = "Lap : " + lap + "/3";
        #endregion
        //If Falling
        if (transform.position.y < 0)
            GetComponent<Rigidbody>().velocity = new Vector3(0, 5, 0);

        // Inputs
        forwardMove = Input.GetAxis("Vertical2");
        turnForce = Input.GetAxis("Horizontal2") * turnRate;

        // Turbo !!!!
		if (Input.GetButton("Accelerator"))
        {
            UseTurbo();
            GameGUI.turboElement = (int)turboElement;
        }

        TurnVehicle();
        Forward();
        

        #region GestionBarresHUD
        UpdateCollisionTime();

        float currentspeed = (speed / maxSpeed) * 160;
        SpeedBar.GetComponent<RectTransform>().sizeDelta = new Vector2(currentspeed/3, 20);
        TurboBar.GetComponent<RectTransform>().sizeDelta = new Vector2(turboElement*2, 20);

        #endregion  
    }

    //Texte Collision
    void SetCollisionHUD(int value, int level)
    {
        if (value <= 160)
        {
            BonneConduite.GetComponent<Image>().color = Color.yellow;
            BonneConduite.GetComponent<RectTransform>().sizeDelta = new Vector2(value, 20);
        }
        if (value > 160 && value <= 320)
        {
            BonneConduite.GetComponent<Image>().color = new Color(1f, 0.5f, 0, 1f);
            BonneConduite.GetComponent<RectTransform>().sizeDelta = new Vector2(value - 160, 20);
        }
        if (value > 320 && value <= 480)
        {
            BonneConduite.GetComponent<Image>().color = Color.red;
            BonneConduite.GetComponent<RectTransform>().sizeDelta = new Vector2(value - 320, 20);
        }
    }
    void UpdateCollisionTime()
    {
        float dist = Time.deltaTime * speed;

        distance += dist;
        int value = (int)(distance * ptByKm / 1000);
        SetCollisionHUD(value, Level(value));
    }


}