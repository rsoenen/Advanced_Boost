using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : VehicleController{

    #region Variables
    private float elapsed = 0.0f;
    private int MyCheckPoint;
    public Text lapText;
    public Text timeElapsedText;
    public Text Position;
    public Text Info;
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
    private NavMeshPath path;
    float template;
    float DistanceHumain;
    bool gettingtime;
    bool possession;
    #endregion

    #region VariableNav
    NavMeshAgent agent;
    ArrayList listTarget;
    int index;
    #endregion


    void Start()
    {
        possession = true;
        gettingtime = true;
        Info.text = "Get Ready!";
        DistanceHumain = 0;
        elapsed = 0.0f;
        path = new NavMeshPath();
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
        BonneConduite = GameObject.FindWithTag("BonneConduite");
        SpeedBar=GameObject.FindWithTag("Vitesse");
        TurboBar=GameObject.FindWithTag("Turbo");
        cp1 = false;
        cp2 = false;
        cp3 = false;
    }

    void FixedUpdate()
    {
        if (GetTime() + 1.5 < Time.time && gettingtime)
        {
            Info.text = "3!";
        }

        if (GetTime() + 2.5 < Time.time && gettingtime)
        {
            Info.text = "2!";
        }
        if (GetTime() + 3.5 < Time.time && gettingtime)
        {
            Info.text = "1!";
        }
        if (GetTime() + 4.5 < Time.time && gettingtime)
        {
            Info.text = "Go!";
        }
        if (GetTime() + 5 < Time.time)
        {
            if(lap==1)
                Info.text = "";
            if (gettingtime)
            {
                template = Time.time;
                gettingtime = false;
            }
            elapsed += Time.deltaTime;
            if (elapsed > 0.2f)
            {
                elapsed -= 0.2f;
                if (index > 0)
                    NavMesh.CalculatePath(transform.position, (Vector3)listTarget[index - 1], NavMesh.AllAreas, path);
                else
                    NavMesh.CalculatePath(transform.position, (Vector3)listTarget[3], NavMesh.AllAreas, path);
            }
            NombreVaisseaux = (raceController.NombreVaisseau() + NombreHumain);
            NombreVaisseauxString = NombreVaisseaux.ToString();
            for (int i = 0; i < NombreVaisseaux - 1; i++)
            {

                ActualPos = 1;
                int check = raceController.NombreCheckpoint(i);
                if (check > MyCheckPoint)
                    ActualPos++;
                else if (check == MyCheckPoint)
                {
                    if (DistanceHumain > raceController.RemainingDistance(i))
                        ActualPos++;
                }
            }
            Position.text = "Position: " + ActualPos + "/" + NombreVaisseauxString;

            #region NavUpdate
            DistanceHumain = PathLength(path);
            if (agent.remainingDistance < 5 && Time.time > Next + 1)
            {
                MyCheckPoint++;
                Next = Time.time;
                moveToNextTarget();
            }
            #endregion
            #region AffichageTimeElapsed
            minute = 0;
            float time = Time.time - template;
            while (time > 60)
            {
                minute++;
                time = time - 60;
            }
            if (minute > 0)
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

            if (possession)
            {
                // Inputs
                forwardMove = Input.GetAxis("Vertical");
                turnForce = Input.GetAxis("Horizontal") * turnRate;

                // Turbo !!!!
                if (Input.GetKey("space"))
                {
                    turboElementActif = true;
                    UseTurbo();
                    GameGUI.turboElement = (int)turboElement;
                }
                else
                {
                    turboElementActif = false;
                }

                TurnVehicle();
                Forward();

            }
            #region GestionBarresHUD
            UpdateCollisionTime();

            float currentspeed = GetComponent<Rigidbody>().velocity.magnitude;
            SpeedBar.GetComponent<RectTransform>().sizeDelta = new Vector2(currentspeed * 10, 20);
            TurboBar.GetComponent<RectTransform>().sizeDelta = new Vector2(turboElement * 2, 20);

            #endregion
        }
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
            cp1 = true;
        }
        else if (other.gameObject == track.checkpoint2)
        {
            if (cp1)
            {
                cp2 = true;
            }
        }
        else if (other.gameObject == track.checkpoint3)
        {
            if (cp1 && cp2)
            {
                cp3 = true;
            }
        }
        else if (other.gameObject == track.finish)
        {
            if (cp1 && cp2 && cp3)
            {
                if (lap < 3)
                {
                    lap++;
                }
                else
                {
                    agent.speed = 10;
                    possession = false;
                    Info.text = "Finish! You are : " + ActualPos + "/"+NombreVaisseauxString;
                }
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
    float PathLength(NavMeshPath path)
    {
        if (path.corners.Length < 2)
            return 0;

        Vector3 previousCorner = path.corners[0];
        float lengthSoFar = 0.0F;
        int i = 1;
        while (i < path.corners.Length)
        {
            Vector3 currentCorner = path.corners[i];
            lengthSoFar += Vector3.Distance(previousCorner, currentCorner);
            previousCorner = currentCorner;
            i++;
        }
        return lengthSoFar;
    }
}