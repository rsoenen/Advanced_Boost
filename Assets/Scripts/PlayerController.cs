﻿using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : VehicleController
{

    #region Variables
    private float elapsed = 0.0f;
    private float elapsedPos = 0.0f;
    public float tempsfinal;
    private int MyCheckPoint;
    //public Text lapText;
    //public Text timeElapsedText;
    //public Text Position;
    //public Text Classement;
    //public Text BonneConduiteText;
    //public Text Info;
    //public Text Speed;
    //private GameObject BonneConduite;
    //private GameObject SpeedBar;
    //private GameObject TurboBar;
    //private GameObject FinishPanel;
    private int[] classementInt;
    private bool IsGettingFinalResult;
    private bool IsImplementingColor;
    private string[] classementString;
    private int[] classementReference;
    private GameController gameController;
    private int NombreVaisseaux;
    private string NombreVaisseauxString;
    private string classementFinal;
    protected bool cp1, cp2, cp3;
    int minute;
    private float Next;
    private int ActualPos;
    private NavMeshPath path;
    float template;
    private float MyTime;
    float DistanceHumain;
    bool gettingtime;
    [HideInInspector]
    public bool isPlayerRunning;
    private int incrementationClassement;
    public int numeroPlayerController;
    private bool IsScoreImplemented;
    private bool IsFirstImplementation;
    #endregion

    #region VariableNav
    NavMeshAgent agent;
    ArrayList listTarget;
    int index;
    #endregion

    public GameObject canvas;

    void Start()
    {
        IsFirstImplementation = true;
        IsImplementingColor = true;
        IsGettingFinalResult = true;
        classementFinal = "";
        classementInt = new int[8];
        classementString = new string[8];
        classementReference = new int[8];
        incrementationClassement = 1;
        IsScoreImplemented = false;
        isPlayerRunning = true;
        gettingtime = true;
        DistanceHumain = 0;
        elapsed = 0.0f;
        elapsedPos = 0.0f;
        #region InstanceNav
        index = 0;
        listTarget = new ArrayList();
        listTarget.Add(track.checkpoint1.transform.position);
        listTarget.Add(track.checkpoint2.transform.position);
        listTarget.Add(track.checkpoint3.transform.position);
        listTarget.Add(track.finish.transform.position);
        #endregion
        ActualPos = 1;
        MyCheckPoint = 0;
        cp1 = false;
        cp2 = false;
        cp3 = false;
    }

    void FixedUpdate()
    {
        if (IsFirstImplementation)
        {
            MyTime = Time.time;
            IsFirstImplementation = false;
            path = new NavMeshPath();
            agent = this.GetComponent<NavMeshAgent>();
            GameObject gameControllerObject = GameObject.FindWithTag("gameController");
            if (gameControllerObject != null)
            {
                gameController = gameControllerObject.GetComponent<GameController>();
            }

            canvas.transform.FindChild("Finish Panel").gameObject.SetActive(false);

        }
        if (IsImplementingColor)
         {
             if (elements == "Feu")
             {
                 canvas.transform.FindChild("TurboBar").FindChild("Bar").GetComponent<Image>().color = new Color(0.97f, 0.23f, 0, 1f);
             }
             if (elements == "Eau")
             {
                 canvas.transform.FindChild("TurboBar").FindChild("Bar").GetComponent<Image>().color = new Color(0, 0.5f, 1f, 1f);
             }
             if (elements == "Tenebre")
             {
                 canvas.transform.FindChild("TurboBar").FindChild("Bar").GetComponent<Image>().color = Color.black;
             }
             if (elements == "Lumiere")
             {
                 canvas.transform.FindChild("TurboBar").FindChild("Bar").GetComponent<Image>().color = new Color(0.96f, 0.71f, 0, 1f);
             }

             canvas.transform.FindChild("TextTurbo").GetComponent<Text>().text += " : " + elements;

             IsImplementingColor = false;
             if (gameController.typeCourse == "Contre la montre")
                 canvas.transform.FindChild("Position").GetComponent<Text>().text = "";

         }
        #region Preparation

        if (MyTime + 2 < Time.time && gettingtime)
        {
            canvas.transform.FindChild("Info").GetComponent<Text>().text = "3";
        }

        if (MyTime + 3 < Time.time && gettingtime)
        {
            canvas.transform.FindChild("Info").GetComponent<Text>().text = "2";
        }
        if (MyTime + 4 < Time.time && gettingtime)
        {
            canvas.transform.FindChild("Info").GetComponent<Text>().text = "1";
            IsScoreImplemented = false;
        }
        #endregion
        if (MyTime + 5 < Time.time)
        {
            canvas.transform.FindChild("Info").GetComponent<Text>().enabled = false;
            if (gettingtime)
            {
                moveToNextTarget();
                template = Time.time;
                gettingtime = false;
            }
            elapsed += Time.deltaTime;
            elapsedPos += Time.deltaTime;
            if (elapsed > 0.05f)
            {
                elapsed -= 0.05f;
                if (index > 0)
                    NavMesh.CalculatePath(this.transform.position, (Vector3)listTarget[index - 1], NavMesh.AllAreas, path);
                else
                    NavMesh.CalculatePath(this.transform.position, (Vector3)listTarget[3], NavMesh.AllAreas, path);
            }
            if (elapsedPos > 0.2f && isPlayerRunning)
            {
                elapsedPos -= 0.2f;
                NombreVaisseaux = (gameController.NombreVaisseau() + gameController.NombreVaisseauHumain());
                NombreVaisseauxString = NombreVaisseaux.ToString();
                #region CalculPos
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
                if (gameController.typeCourse != "Contre la montre")
                    canvas.transform.FindChild("Position").GetComponent<Text>().text = "Position: " + ActualPos + "/" + NombreVaisseauxString;
                else
                    canvas.transform.FindChild("Position").GetComponent<Text>().text = "";
            }
            #endregion
            #region NavUpdate
            DistanceHumain = PathLength(path);
            if (agent.remainingDistance < 5 && Time.time > Next + 1 && (Time.time - MyTime) > 7)
            {
                moveToNextTarget();
                MyCheckPoint++;
                Next = Time.time;
            }
            #endregion
            #region AffichageCompteurTour
            canvas.transform.FindChild("Tour").GetComponent<Text>().text = "Lap : " + lap + "/3";
            #endregion
            //If Falling
            if (transform.position.y < 0)
                GetComponent<Rigidbody>().velocity = new Vector3(0, 5, 0);

            if (isPlayerRunning)
            {

                #region AffichageTimeElapsed
                float time = Time.time - template;

                minute = (int)(time) / 60;
                float seconds = time % 60;

                if (minute > 0)
                    canvas.transform.FindChild("Time").GetComponent<Text>().text = "Time: " + minute.ToString() + ":" + seconds.ToString("00.000");
                else
                    canvas.transform.FindChild("Time").GetComponent<Text>().text = "Time: " + seconds.ToString("0:00.000");

                if (isPlayerRunning)
                {
                    tempsfinal = Time.time - template;
                }
                #endregion
                float currentspeed = GetComponent<Rigidbody>().velocity.magnitude;
                canvas.transform.FindChild("SpeedBar").FindChild("Bar").GetComponent<RectTransform>().sizeDelta = new Vector2(currentspeed / (maxSpeed + 4 + speed / 100 * Boost(Level((int)distance))) * 160, 20);
                canvas.transform.FindChild("TextSpeed").GetComponent<Text>().text = "Vitesse: " + (currentspeed * 49).ToString("00.0");
                // Inputs
                //CLAVIER
                if (numeroPlayerController == 1)
                {
                    forwardMove = Input.GetAxis("Vertical");
                    turnForce = Input.GetAxis("Horizontal") * turnRate;

                    // Turbo !!!!
                    if (Input.GetKey("space"))
                    {
                        turboElementActif = true;
                        UseTurbo();
                    }
                    else
                    {
                        turboElementActif = false;
                    }
                }
                if (numeroPlayerController == 2)
                {
                    forwardMove = Input.GetAxis("Vertical2");
                    turnForce = Input.GetAxis("Horizontal2") * turnRate;

                    // Turbo !!!!
                    if (Input.GetButton("Accelerator2"))
                    {
                        turboElementActif = true;
                        UseTurbo();
                    }
                    else
                    {
                        turboElementActif = false;
                    }
                }
                if (numeroPlayerController == 3)
                {
                    forwardMove = Input.GetAxis("Vertical3");
                    turnForce = Input.GetAxis("Horizontal3") * turnRate;

                    // Turbo !!!!
                    if (Input.GetButton("Accelerator3"))
                    {
                        turboElementActif = true;
                        UseTurbo();
                    }
                    else
                    {
                        turboElementActif = false;
                    }
                }
                if (numeroPlayerController == 4)
                {
                    forwardMove = Input.GetAxis("Vertical4");
                    turnForce = Input.GetAxis("Horizontal4") * turnRate;

                    // Turbo !!!!
                    if (Input.GetButton("Accelerator4"))
                    {
                        turboElementActif = true;
                        UseTurbo();
                    }
                    else
                    {
                        turboElementActif = false;
                    }
                }
                TurnVehicle();
                Forward();

            }
            else
            {
                GameObject gameControllerObject = GameObject.FindWithTag("gameController");
                GameController g = gameControllerObject.GetComponent<GameController>();
                try
                {
                    for (int i = 0; i < gameController.nombreIA; i++)
                    {
                        classementInt[i] = gameController.PosIA(i);
                        classementString[i] = gameController.MyAirships[i].tag;
                        if(g.typeCourse== "Championnat" && !IsScoreImplemented)
                        {
                            g.PointDuChampionnat[i] += g.PointParPosition[gameController.PosIA(i) - 1];
                            classementReference[i] = i ;
                        }
                    }
                }
                catch (ArgumentOutOfRangeException)
                {

                }
                try
                {
                    for (int i = 0; i < gameController.NombreVaisseauHumain(); i++)
                    {
                        classementInt[i + gameController.nombreIA] = gameController.PosHumain(i);
                        classementString[i + gameController.nombreIA] = gameController.MyAirshipsHumain[i].tag;
                        if (g.typeCourse == "Championnat" && i==0 && !IsScoreImplemented)
                        {
                            g.PointDuChampionnat[i + gameController.nombreIA] += g.PointParPosition[gameController.PosHumain(i)-1];
                            classementReference[i + gameController.nombreIA] = i + gameController.nombreIA;
                        }
                    }
                }
                catch (ArgumentOutOfRangeException)
                {

                }
                IsScoreImplemented=true;
                if (IsGettingFinalResult)
                {
                    if (g.currentTrack == 4 && g.typeCourse == "Championnat")
                        classementFinal = "Classement Final:\n";
                    else
                        classementFinal = "Classement:\n";
                }
                
                canvas.transform.FindChild("Finish Panel").gameObject.SetActive(true);

                incrementationClassement = 1;
                while (incrementationClassement < 9)
                {
                    int MaxScore = 0;
                    int indexMax = 0;
                    for (int i = 0; i < 8; i++)
                    {
                        if (g.currentTrack < 4)
                        {
                            if (classementInt[i] == incrementationClassement)
                            {
                                if (g.typeCourse == "Championnat")
                                {
                                    classementFinal += classementInt[i] + ". " + classementString[i] + "/ Points:" + g.PointDuChampionnat[classementReference[i]] + "\n";
                                }
                                else
                                    classementFinal += classementInt[i] + ". " + classementString[i] + "\n";
                            }
                        }
                        else
                        {
                            if (g.PointDuChampionnat[i] > MaxScore)
                            {
                                MaxScore = g.PointDuChampionnat[i];
                                indexMax = i;
                            }
                        }
                    }
                    if (g.typeCourse == "Championnat" && g.currentTrack == 4 && IsGettingFinalResult)
                    {
                        classementFinal += incrementationClassement + ". " + classementString[indexMax] + "/ Points:" + MaxScore + "\n";
                        g.PointDuChampionnat[indexMax] = 0;
                        if (incrementationClassement == 8)
                            IsGettingFinalResult = false;
                    }
                    incrementationClassement++;
                }
                canvas.transform.FindChild("Finish Panel").FindChild("Classement").GetComponent<Text>().text = classementFinal;
            }
            #region GestionBarresHUD
            UpdateCollisionTime();
            canvas.transform.FindChild("TurboBar").FindChild("Bar").GetComponent<RectTransform>().sizeDelta = new Vector2(turboElement * 16 / 10, 20);

            #endregion
        }

    }

    //Texte Collision
    void SetCollisionHUD(int value, int level)
    {
        if (value <= 160)
        {
            canvas.transform.FindChild("GoodDriveBar").FindChild("Bar").GetComponent<Image>().color = Color.yellow;
            canvas.transform.FindChild("GoodDriveBar").FindChild("Bar").GetComponent<RectTransform>().sizeDelta = new Vector2(value, 20);
            canvas.transform.FindChild("TextBonneConduite").GetComponent<Text>().text = "Bonne Conduite Niveau 1";
        }
        if (value > 160 && value <= 320)
        {
            canvas.transform.FindChild("GoodDriveBar").FindChild("Bar").GetComponent<Image>().color = new Color(1f, 0.5f, 0, 1f);
            canvas.transform.FindChild("GoodDriveBar").FindChild("Bar").GetComponent<RectTransform>().sizeDelta = new Vector2(value - 160, 20);
            canvas.transform.FindChild("TextBonneConduite").GetComponent<Text>().text = "Bonne Conduite Niveau 2";
        }
        if (value > 320 && value <= 480)
        {
            canvas.transform.FindChild("GoodDriveBar").FindChild("Bar").GetComponent<Image>().color = Color.red;
            canvas.transform.FindChild("GoodDriveBar").FindChild("Bar").GetComponent<RectTransform>().sizeDelta = new Vector2(value - 320, 20);
            canvas.transform.FindChild("TextBonneConduite").GetComponent<Text>().text = "Bonne Conduite Niveau 3";
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
                    isPlayerRunning = false;
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