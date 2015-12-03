using UnityEngine;
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
    public Text lapText;
    public Text timeElapsedText;
    public Text Position;
    public Text Classement;
    private GameObject BonneConduite;
    private GameObject SpeedBar;
    private GameObject TurboBar;
    private GameObject FinishPanel;
    private int[] classementInt;
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
    #endregion

    #region VariableNav
    NavMeshAgent agent;
    ArrayList listTarget;
    int index;
    #endregion


    void Start()
    {
        classementFinal = "";
        classementInt = new int[8];
        classementString = new string[8];
        classementReference = new int[8];
        incrementationClassement = 1;
        FinishPanel = GameObject.Find("Finish Panel");
        FinishPanel.SetActive(false);
        IsScoreImplemented = false;
        isPlayerRunning = true;
        gettingtime = true;
        timeElapsedText.text = "Get Ready !";
        DistanceHumain = 0;
        elapsed = 0.0f;
        elapsedPos = 0.0f;
        path = new NavMeshPath();
        #region InstanceNav
        index = 0;
        MyTime = Time.time;
        agent = GetComponent<NavMeshAgent>();
        listTarget = new ArrayList();
        listTarget.Add(track.checkpoint1.transform.position);
        listTarget.Add(track.checkpoint2.transform.position);
        listTarget.Add(track.checkpoint3.transform.position);
        listTarget.Add(track.finish.transform.position);
        #endregion
        ActualPos = 1;
        MyCheckPoint = 0;
        GameObject gameControllerObject = GameObject.FindWithTag("gameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        BonneConduite = GameObject.FindWithTag("BonneConduite");
        SpeedBar = GameObject.FindWithTag("Vitesse");
        TurboBar = GameObject.FindWithTag("Turbo");
        cp1 = false;
        cp2 = false;
        cp3 = false;
    }

    void FixedUpdate()
    {
        #region Preparation
        if (MyTime + 2 < Time.time && gettingtime)
        {
            timeElapsedText.text = "3";
        }

        if (MyTime + 3 < Time.time && gettingtime)
        {
            timeElapsedText.text = "2";
        }
        if (MyTime + 4 < Time.time && gettingtime)
        {
            timeElapsedText.text = "1";
            IsScoreImplemented = false;
        }
        #endregion
        if (MyTime + 5 < Time.time)
        {
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
                    NavMesh.CalculatePath(transform.position, (Vector3)listTarget[index - 1], NavMesh.AllAreas, path);
                else
                    NavMesh.CalculatePath(transform.position, (Vector3)listTarget[3], NavMesh.AllAreas, path);
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
                Position.text = "Position: " + ActualPos + "/" + NombreVaisseauxString;
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
            if (isPlayerRunning)
            {
                tempsfinal = Time.time - template;
            }
            #endregion
            #region AffichageCompteurTour
            lapText.text = "Lap : " + lap + "/3";
            #endregion
            //If Falling
            if (transform.position.y < 0)
                GetComponent<Rigidbody>().velocity = new Vector3(0, 5, 0);

            if (isPlayerRunning)
            {
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
                classementFinal = "Classement:\n";
                FinishPanel.SetActive(true);
                incrementationClassement = 1;
                while (incrementationClassement < 9)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        if (classementInt[i] == incrementationClassement)
                        {
                            if(g.typeCourse=="Championnat")
                                classementFinal += classementInt[i] + ". " + classementString[i] + "/ Points:" +g.PointDuChampionnat[classementReference[i]] +"\n";
                            else
                             classementFinal += classementInt[i] + ". " + classementString[i] + "\n";
                        }
                    }
                    incrementationClassement++;
                }
                Classement.text = classementFinal;
            }
            #region GestionBarresHUD
            UpdateCollisionTime();
            float currentspeed = GetComponent<Rigidbody>().velocity.magnitude;
            SpeedBar.GetComponent<RectTransform>().sizeDelta = new Vector2(currentspeed * 10, 20);
            TurboBar.GetComponent<RectTransform>().sizeDelta = new Vector2(turboElement * 16 / 10, 20);

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