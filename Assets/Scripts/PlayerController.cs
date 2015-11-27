using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : VehicleController{

    #region Variables
    private float elapsed = 0.0f;
    private float elapsedPos = 0.0f;
    private int MyCheckPoint;
    public Text lapText;
    public Text timeElapsedText;
    public Text Position;
    public Text Info;
    public Text Classement;
    private string pos1="";
    private string pos2 = "";
    private string pos3 = "";
    private string pos4 = "";
    private string pos5 = "";
    private string pos6 = "";
    private string pos7 = "";
    private string pos8 = "";
    private GameObject BonneConduite;
    private GameObject SpeedBar;
    private GameObject TurboBar;
    private gameController gameController;
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
        Classement.text = "";
        possession = true;
        gettingtime = true;
        Info.text = "Get Ready!";
        DistanceHumain = 0;
        elapsed = 0.0f;
        elapsedPos = 0.0f;
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
        GameObject gameControllerObject = GameObject.FindWithTag("gameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<gameController>();
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
                if (lap == 1)
                    Info.text = "";
                if (gettingtime)
                {
                    template = Time.time;
                    gettingtime = false;
                }
                elapsed += Time.deltaTime;
                elapsedPos += Time.deltaTime;
                if (elapsed > 0.05f)
                {
                    elapsed -= 0.05f;
                    if (index > 0)
                        NavMesh.CalculatePath(transform.position, (Vector3)listTarget[index - 1], NavMesh.AllAreas, path);
                    else
                        NavMesh.CalculatePath(transform.position, (Vector3)listTarget[3], NavMesh.AllAreas, path);

                }
                if (elapsedPos > 0.2f && possession)
                {
                    elapsedPos -= 0.2f;
                    NombreVaisseaux = (gameController.NombreVaisseau() + gameController.NombreVaisseauHumain());
                    NombreVaisseauxString = NombreVaisseaux.ToString();

                    ActualPos = 1;
                    for (int i = 0; i < gameController.NombreVaisseau(); i++)
                    {
                        int check = gameController.NombreCheckpoint(i);
                        if (check > MyCheckPoint)
                            ActualPos++;
                        else if (check == MyCheckPoint)
                        {
                            if (DistanceHumain > gameController.RemainingDistance(i))
                                ActualPos++;
                        }
                    }
                    for (int i = 0; i < gameController.NombreVaisseauHumain(); i++)
                    {
                        if (gameController.MyAirshipsHumain[i].tag != tag)
                        {
                            int check = gameController.NombreCheckpointHumain(i);
                            if (check > MyCheckPoint)
                                ActualPos++;
                            else if (check == MyCheckPoint)
                            {
                                if (DistanceHumain > gameController.RemainingDistanceHumain(i))
                                    ActualPos++;
                            }
                        }
                    }
                    Position.text = "Position: " + ActualPos + "/" + NombreVaisseauxString;
                }
                #region NavUpdate
                DistanceHumain = PathLength(path);
                if (agent.remainingDistance < 5 && Time.time > Next + 1)
                {
                    moveToNextTarget();
                    MyCheckPoint++;
                    Next = Time.time;
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
                else
                {
                    for (int i = 0; i < gameController.NombreVaisseau(); i++)
                    {
                        if (gameController.PosIA(i) == 1 && NombreVaisseaux > 0)
                        {
                            pos1 = "1) " + gameController.MyAirships[i].tag;
                        }
                        if (gameController.PosIA(i) == 2 && NombreVaisseaux > 1)
                        {
                            pos2 = "2) " + gameController.MyAirships[i].tag;
                        }
                        if (gameController.PosIA(i) == 3 && NombreVaisseaux > 2)
                        {
                            pos3 = "3) " + gameController.MyAirships[i].tag;
                        }
                        if (gameController.PosIA(i) == 4 && NombreVaisseaux > 3)
                        {
                            pos4 = "4) " + gameController.MyAirships[i].tag;
                        }
                        if (gameController.PosIA(i) == 5 && NombreVaisseaux > 4)
                        {
                            pos5 = "5) " + gameController.MyAirships[i].tag;
                        }
                        if (gameController.PosIA(i) == 6 && NombreVaisseaux > 5)
                        {
                            pos6 = "6) " + gameController.MyAirships[i].tag;
                        }
                        if (gameController.PosIA(i) == 7 && NombreVaisseaux > 6)
                        {
                            pos7 = "7) " + gameController.MyAirships[i].tag;
                        }
                        if (gameController.PosIA(i) == 8 && NombreVaisseaux > 7)
                        {
                            pos8 = "8) " + gameController.MyAirships[i].tag;
                        }
                    }
                    for (int i = 0; i < gameController.NombreVaisseauHumain(); i++)
                    {

                        if (gameController.PosHumain(i) == 1 && NombreVaisseaux > 0)
                        {
                            pos1 = "1) " + gameController.MyAirshipsHumain[i].tag;
                        }
                        if (gameController.PosHumain(i) == 2 && NombreVaisseaux > 1)
                        {
                            pos2 = "2) " + gameController.MyAirshipsHumain[i].tag;
                        }
                        if (gameController.PosHumain(i) == 3 && NombreVaisseaux > 2)
                        {
                            pos3 = "3) " + gameController.MyAirshipsHumain[i].tag;
                        }
                        if (gameController.PosHumain(i) == 4 && NombreVaisseaux > 3)
                        {
                            pos4 = "4) " + gameController.MyAirshipsHumain[i].tag;
                        }
                        if (gameController.PosHumain(i) == 5 && NombreVaisseaux > 4)
                        {
                            pos5 = "5) " + gameController.MyAirshipsHumain[i].tag;
                        }
                        if (gameController.PosHumain(i) == 6 && NombreVaisseaux > 5)
                        {
                            pos6 = "6) " + gameController.MyAirshipsHumain[i].tag;
                        }
                        if (gameController.PosHumain(i) == 7 && NombreVaisseaux > 6)
                        {
                            pos7 = "7) " + gameController.MyAirshipsHumain[i].tag;
                        }
                        if (gameController.PosHumain(i) == 8 && NombreVaisseaux > 7)
                        {
                            pos8 = "8) " + gameController.MyAirshipsHumain[i].tag;
                        }
                    }
                    Classement.text = "Classement Final:\n" + pos1 + "\n" + pos2 + "\n" + pos3 + "\n" + pos4 + "\n" + pos5 + "\n" + pos6 + "\n" + pos7 + "\n" + pos8 + "\n";



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
                    agent.acceleration = 3;
                    agent.angularSpeed = 50;
                    agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
                    agent.avoidancePriority = 50;
                    agent.radius = 0.8428867f;
                    agent.height = 0.3234494f;
                    agent.baseOffset = 0.1617247f;
                    agent.autoRepath = true;
                    agent.autoBraking = true;
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
    public int NombreCheckPoint()
    {
        return MyCheckPoint;
    }
    public float RemainingDis()
    {
        return DistanceHumain;
    }
    public int PositionHumain()
    {
        return ActualPos;
    }
}