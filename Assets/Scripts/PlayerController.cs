using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : VehicleController{

    #region Variables
    private int MyCheckPoint;
    public Text lapText;
    public Text timeElapsedText;
    public Text Position;
    private GameObject BonneConduite;
    private GameObject SpeedBar;
    private GameObject TurboBar;
    private RaceController raceController;
    private int NombreHumain = 1;
    private int NombreVaisseaux;
    private string NombreVaisseauxString;
    protected bool cp1, cp2, cp3;
    int minute;
    private float Next;
    private int ActualPos;

    #endregion

    #region VariableNav
    NavMeshAgent agent;
    ArrayList listTarget;
    int index;
    #endregion


    void Start()
    {

        #region InstanceNav
        index = 0;
        agent = GetComponent<NavMeshAgent>();
        listTarget = new ArrayList();
        listTarget.Add(track.checkpoint1.transform.position);
        listTarget.Add(track.checkpoint2.transform.position);
        listTarget.Add(track.checkpoint3.transform.position);
        listTarget.Add(track.finish.transform.position);
        moveToNextTarget();
        #endregion
        ActualPos = 1;
        MyCheckPoint = 0;
        GameObject raceControllerObject = GameObject.FindWithTag("RaceController");
        if (raceControllerObject != null)
        {
            raceController = raceControllerObject.GetComponent<RaceController>();
        }
        NombreVaisseaux = (raceController.NombreVaisseau() + NombreHumain);
        NombreVaisseauxString = NombreVaisseaux.ToString();
        Position.text = "Position: 1/" + NombreVaisseauxString;
        BonneConduite = GameObject.FindWithTag("BonneConduite");
        SpeedBar=GameObject.FindWithTag("Vitesse");
        TurboBar=GameObject.FindWithTag("Turbo");
        cp1 = false;
        cp2 = false;
        cp3 = false;
    }

    void FixedUpdate()
    {
        #region NavUpdate
        if (agent.remainingDistance < 5 && Time.time>Next+2)
        {
            Next = Time.time;
            moveToNextTarget();
        }
        #endregion
        for (int i = 0; i < NombreVaisseaux-1; i++)
        {
            
            ActualPos = 1;
            int check= raceController.NombreCheckpoint(i);
            if (check > MyCheckPoint)
                ActualPos++;
            if (check ==MyCheckPoint)
            {
                if(agent.remainingDistance>999 || raceController.Distance(i)>999)
                {
                    float x1;
                    float z1;
                    if (index > 0)
                    {
                        x1 = Math.Abs(transform.position.x) - Math.Abs(((Vector3)listTarget[index - 1]).x);
                        z1 = Math.Abs(transform.position.z) - Math.Abs(((Vector3)listTarget[index - 1]).z);
                    }
                    else
                    {
                        x1 = Math.Abs(transform.position.x) - Math.Abs(((Vector3)listTarget[3]).x);
                        z1 = Math.Abs(transform.position.z) - Math.Abs(((Vector3)listTarget[3]).z);
                    }
                    float x2 = Math.Abs(raceController.Pos(i).x) - Math.Abs((raceController.Actual(i)).x);
                    float z2 = Math.Abs(raceController.Pos(i).z) - Math.Abs((raceController.Actual(i)).z);

                    if ((x1 * x1 + z1 * z1) > (x2 * x2 + z2 * z2))
                    {
                        ActualPos++;
                    }
                    
                }
                else if(agent.remainingDistance>raceController.Distance(i))
                {
                    ActualPos++;
                }
            }
                Position.text = "Position: " +  ActualPos + "/" + NombreVaisseauxString;
        }
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
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == track.checkpoint1)
        {
            if (!cp1)
                MyCheckPoint++;
            cp1 = true;
        }
        else if (other.gameObject == track.checkpoint2)
        {
            if (cp1 && !cp2)
            {
                cp2 = true;
                MyCheckPoint++;
            }
        }
        else if (other.gameObject == track.checkpoint3)
        {
            if (cp1 && cp2 && !cp3)
            {
                cp3 = true;
                MyCheckPoint++;
            }
        }
        else if (other.gameObject == track.finish)
        {
            if (cp1 && cp2 && cp3)
            {
                lap++;
                MyCheckPoint++;
            }
            cp1 = false;
            cp2 = false;
            cp3 = false;
        }
    }

     void moveToNextTarget()
    {
        agent.SetDestination((Vector3)listTarget[index]);
        index++;
        if (index >= listTarget.Count)
            index = 0;
    }
}