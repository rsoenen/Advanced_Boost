using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : VehicleController{

    #region Variables
    bool checkpoint1;
    bool checkpoint2;
    bool checkpoint3;
    public Text text;
    public Text TimeElapsed;
    private GameObject BonneConduite;
    private GameObject SpeedBar;
    private GameObject TurboBar;
    double x11;
    double x12;
    double x21;
    double x22;
    double x31;
    double x32;
    double z11;
    double z12;
    double z21;
    double z22;
    double z31;
    double z32;
    double fx1;
    double fx2;
    double fz1;
    double fz2;
    int Lap;
    int min;
    #endregion
    void Start()
    {
        BonneConduite = GameObject.FindWithTag("BonneConduite");
        SpeedBar=GameObject.FindWithTag("Vitesse");
        TurboBar= GameObject.FindWithTag("Turbo");
        Lap = 1;
        GameGUI.maxTurboElement = (int)maxTurboElement;
        checkpoint1=false;
        checkpoint2=false;
        checkpoint3=false;
    }

    void FixedUpdate()
    {
        #region AffichageTimeElapsed
        min = 0;
        float time = Time.time;
        while(time>60)
        {
            min++;
            time = time - 60;
        }
        if(min>0)
            TimeElapsed.text = "Time: " + min.ToString() + ":" + time.ToString("00.000");
        else
            TimeElapsed.text = "Time: " + time.ToString("0:00.000");
        #endregion
        //if falling
        if (transform.position.y < 0)
            GetComponent<Rigidbody>().velocity = new Vector3(0, 5, 0);
        // Inputs
        forwardMove = Input.GetAxis("Vertical");
        turnForce = Input.GetAxis("Horizontal") * turnRate;

        // Turbo !!!!
        if (Input.GetKey("space"))
        {
            UseTurbo();
            GameGUI.turboElement = (int)turboElement;
        }

        TurnVehicle();
        Forward();
        UpdateCollisionTime();
        #region GestionBarresHUD
        float currentspeed = (speed / maxSpeed) * 160;
        SpeedBar.GetComponent<RectTransform>().anchoredPosition = new Vector3(-180+ (currentspeed / 6), 41, 0);
        SpeedBar.GetComponent<RectTransform>().sizeDelta = new Vector2(currentspeed/3, 20);
        TurboBar.GetComponent<RectTransform>().anchoredPosition = new Vector3(-95+ (turboElement), 41, 0);
        TurboBar.GetComponent<RectTransform>().sizeDelta = new Vector2(turboElement*2, 20);
        #endregion
        #region Map1
        if (Application.loadedLevel == 1)
        {
            x11 = 47;
            x12 = 50;
            x21 = 16;
            x22 = 24;
            x31 = -34;
            x32 = -30;
            z11 = -4;
            z12 = 0;
            z21 = -51;
            z22 = -44;
            z31 = -80;
            z32 = -76;
            fx1 = 0.5;
            fx2 = 1.5;
            fz1 = -2;
            fz2 = 2;
        }
        #endregion
        #region Map2
        if (Application.loadedLevel == 2)
        {
            x11 = 39;
            x12 = 45;
            x21 = 39;
            x22 = 41;
            x31 = -23;
            x32 = -18;
            z11 = 7;
            z12 = 13;
            z21 = 74;
            z22 = 77;
            z31 = 61;
            z32 = 66;
            fx1 = -2;
            fx2 = -1;
            fz1 = -2;
            fz2 = 2;
        }
        #endregion
        #region Map3
        if (Application.loadedLevel == 3)
        {
            x11 = 18;
            x12 = 24;
            x21 = -8.5;
            x22 = -3.5;
            x31 = -49;
            x32 = -45;
            z11 = -22;
            z12 = -18;
            z21 = -51;
            z22 = -49;
            z31 = -30;
            z32 = -28;
            fx1 = 1.5;
            fx2 = 2.5;
            fz1 = -2;
            fz2 = 2;
        }
        #endregion
        #region Map4
        if (Application.loadedLevel == 4)
        {
            x11 = 41;
            x12 = 45;
            x21 = -13;
            x22 = -11;
            x31 = -13;
            x32 = -11;
            z11 = -21;
            z12 = -19;
            z21 = -26;
            z22 = -22;
            z31 = -14;
            z32 = -10;
            fx1 = -2.5;
            fx2 = -1;
            fz1 = -2;
            fz2 = 2;
        }
        #endregion
        #region Map5
        if (Application.loadedLevel == 5)
        {
            x11 = 21;
            x12 = 26;
            x21 = 40;
            x22 = 43;
            x31 = -36;
            x32 = -32;
            z11 = 33;
            z12 = 36;
            z21 = -17.5;
            z22 = -13;
            z31 = -17.5;
            z32 = -13;
            fx1 = -17.5;
            fx2 = -16.5;
            fz1 = 2;
            fz2 = 6;
        }
        #endregion
        #region GestionCheckpoint
        if ((transform.position.x > x11 && transform.position.x < x12) && (transform.position.z > z11 && transform.position.z < z12))
        {
            checkpoint1 = true;
        }
        if ((transform.position.x > x21 && transform.position.x < x22) && (transform.position.z > z21 && transform.position.z < z22))
        {
            checkpoint2 = true;
        }
        if ((transform.position.x > x31 && transform.position.x < x32) && (transform.position.z > z31 && transform.position.z < z32))
        {
            checkpoint3 = true;
        }
        if ((transform.position.x > fx1 && transform.position.x < fx2) && (transform.position.z > fz1 && transform.position.z < fz2))
        {
            if (checkpoint3 && checkpoint2 && checkpoint1)
            {

                Lap++;
                text.text = "Lap: " + Lap + "/3";
            }
            checkpoint1 = false;
            checkpoint2 = false;
            checkpoint3 = false;
        }

        if (checkpoint3 && !checkpoint1)
        {
            checkpoint3 = false;
        }
        #endregion
    }

    // Texte Collision
    public Text collisionText;
    void SetCollisionText(int value, int level)
    {
        collisionText.text = "Distance (in m) since last collision : " + value.ToString();
        #region BonneConduite
        if (value <= 160)
        {
            BonneConduite.GetComponent<Image>().color = new Color(1f, 1f, 0, 1f);
            BonneConduite.GetComponent<RectTransform>().sizeDelta = new Vector2(value, 20);
            BonneConduite.GetComponent<RectTransform>().anchoredPosition = new Vector3(16+ (value / 2), 41, 0);
        }
        if (value > 160 && value <= 320)
        {
            BonneConduite.GetComponent<Image>().color = new Color(1f, 0.5f, 0, 1f);
            BonneConduite.GetComponent<RectTransform>().sizeDelta = new Vector2(value - 160, 20);
            BonneConduite.GetComponent<RectTransform>().anchoredPosition = new Vector3(16 + ((value - 160) / 2), 41, 0);
        }
        if (value > 320 && value <= 480)
        {
            BonneConduite.GetComponent<Image>().color = new Color(1f, 0, 0, 1f);
            BonneConduite.GetComponent<RectTransform>().sizeDelta = new Vector2(value - 320, 20);
            BonneConduite.GetComponent<RectTransform>().anchoredPosition = new Vector3(16+ ((value - 320) / 2), 41, 0);
        }
        #endregion
    }
    void UpdateCollisionTime()
    {
        float dist = Time.deltaTime * speed;

        distance += dist;
        int value = (int)(distance * ptByKm / 1000);
        SetCollisionText(value, Level(value));
    }


}