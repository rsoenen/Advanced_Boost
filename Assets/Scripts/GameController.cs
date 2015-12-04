using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{

    private int nombreVaisseau;
    public int nombreIA;
    public Transform airshipEnemy;
    public Transform Vehicule1ennemy;
    public Transform airshipAlly;
    private float temps;
    public string element;
    public string typeCourse;
    private Championnat championnat;
    public int CourseActuelDuChampionnat;
    public List<GameObject> MyAirships;
    public List<GameObject> MyAirshipsHumain;
    public List<Vaisseau> ParticipantChampionnat;
    public int[] PointDuChampionnat;
    public int[] PointParPosition;

    // Use this for initialization
    void Start()
    {
        //debutCourse();
        PointDuChampionnat = new int[8];
        PointParPosition = new int[8];
        PointParPosition[0] = 20;
        PointParPosition[1] = 15;
        PointParPosition[2] = 12;
        PointParPosition[3] = 8;
        PointParPosition[4] = 6;
        PointParPosition[5] = 4;
        PointParPosition[6] = 2;
        PointParPosition[7] = 0;
        temps = 0;
        nombreVaisseau = 8;
        nombreIA = 7;
        CourseActuelDuChampionnat = 1;
        MyAirships = new List<GameObject>();
        MyAirshipsHumain = new List<GameObject>();
        ParticipantChampionnat = new List<Vaisseau>();
    }

    // Update is called once per frame
    void Update()
    {
    }


    public void startCourse()
    {

        if (typeCourse == "Contre la montre")
        {
            debutContreLaMontre();
        }
        else
        {
            debutCourse();
            if (typeCourse == "Championnat" && championnat == null)
            {
                //championnat = new Championnat(ParticipantChampionnat, GetComponent<StartOptions>().getMapLoad());
            }
        }
    }

    public void debutContreLaMontre()
    {
        Quaternion rotate = Quaternion.Euler(0, 90, 0);
        Track trackCircuit = GameObject.Find("Track").GetComponent<Track>();

        Text position = GameObject.Find("Position").GetComponent<Text>();
        Text tour = GameObject.Find("Tour").GetComponent<Text>();
        Text time = GameObject.Find("Time").GetComponent<Text>();
        Text classement = GameObject.Find("Classement").GetComponent<Text>();
        Text bonneconduite = GameObject.Find("TextBonneConduite").GetComponent<Text>();

        Vector3 pos = GameObject.Find("Spawn1").transform.position;
        Transform player = (Transform)Instantiate(airshipAlly, pos, rotate);
        player.GetComponent<PlayerController>().track = trackCircuit;
        player.GetComponent<PlayerController>().lapText = tour;
        player.GetComponent<PlayerController>().timeElapsedText = time;
        player.GetComponent<PlayerController>().Position = position;
        player.GetComponent<PlayerController>().Classement = classement;
        player.GetComponent<PlayerController>().numeroPlayerController = 1;
        player.GetComponent<PlayerController>().elements = element;
        player.GetComponent<PlayerController>().BonneConduiteText = bonneconduite;

    }

    public void debutCourse()
    {
        Track trackCircuit = GameObject.Find("Track").GetComponent<Track>();
        Quaternion rotate = Quaternion.Euler(0, 90, 0);

        for (int i = 1; i < nombreIA + 1; i++)
        {
            Vector3 pos = GameObject.Find("Spawn" + i).transform.position;
            float rand = Random.value;
            if (rand < 0.5f)
            {
                Transform ennemy = (Transform)Instantiate(airshipEnemy, pos, rotate);
                ennemy.GetComponent<EnemyController>().track = trackCircuit;
                ennemy.tag = "VaisseauEnnemi" + i;
                MyAirships.Add(ennemy.gameObject);
            }
            else
            {
                Transform ennemy = (Transform)Instantiate(Vehicule1ennemy, pos, rotate);
                ennemy.GetComponent<EnemyController>().track = trackCircuit;
                ennemy.tag = "VaisseauEnnemi" + i;
                MyAirships.Add(ennemy.gameObject);
            }
        }

        Text position = GameObject.Find("Position").GetComponent<Text>();
        Text tour = GameObject.Find("Tour").GetComponent<Text>();
        Text time = GameObject.Find("Time").GetComponent<Text>();
        Text classement = GameObject.Find("Classement").GetComponent<Text>();
        Text bonneconduite = GameObject.Find("TextBonneConduite").GetComponent<Text>();

        int nombreJoueur = nombreVaisseau - nombreIA;
        for (int i = 1; i < nombreJoueur + 1; i++)
        {
            int numberSpawn = nombreIA + i;
            Vector3 pos = GameObject.Find("Spawn" + numberSpawn).transform.position;
            Transform player = (Transform)Instantiate(airshipAlly, pos, rotate);
            player.GetComponent<PlayerController>().track = trackCircuit;
            player.GetComponent<PlayerController>().lapText = tour;
            player.GetComponent<PlayerController>().timeElapsedText = time;
            player.GetComponent<PlayerController>().Position = position;
            player.GetComponent<PlayerController>().Classement = classement;
            player.GetComponent<PlayerController>().numeroPlayerController = i;
            player.GetComponent<PlayerController>().elements = element;
            player.GetComponent<PlayerController>().BonneConduiteText = bonneconduite;
            if (nombreJoueur == 1)
            {
                player.Find("MainCamera").GetComponent<Camera>().rect = new Rect(0f, 0f, 1f, 1f);
            }
            if (nombreJoueur == 2)
            {
                if (i == 1)
                {
                    player.Find("MainCamera").GetComponent<Camera>().rect = new Rect(0f, 0f, 1f, .5f);
                }
                if (i == 2)
                {
                    player.Find("MainCamera").GetComponent<Camera>().rect = new Rect(0f, .5f, 1f, .5f);
                }
            }
            if (nombreJoueur == 3)
            {
                if (i == 1)
                {
                    player.Find("MainCamera").GetComponent<Camera>().rect = new Rect(0f, .5f, .5f, .5f);
                }
                if (i == 2)
                {
                    player.Find("MainCamera").GetComponent<Camera>().rect = new Rect(.5f, .5f, .5f, .5f);
                }
                if (i == 3)
                {
                    player.Find("MainCamera").GetComponent<Camera>().rect = new Rect(.25f, 0f, .5f, .5f);
                }
            }
            if (nombreJoueur == 4)
            {
                if (i == 1)
                {
                    player.Find("MainCamera").GetComponent<Camera>().rect = new Rect(0f, .5f, .5f, .5f);
                }
                if (i == 2)
                {
                    player.Find("MainCamera").GetComponent<Camera>().rect = new Rect(.5f, .5f, .5f, .5f);
                }
                if (i == 3)
                {
                    player.Find("MainCamera").GetComponent<Camera>().rect = new Rect(0f, 0f, .5f, .5f);
                }
                if (i == 4)
                {
                    player.Find("MainCamera").GetComponent<Camera>().rect = new Rect(.5f, 0f, .5f, .5f);
                }
            }


            player.tag = "Player" + i;
            MyAirshipsHumain.Add(player.gameObject);
        }


    }

    public int getNombreVaisseau()
    {
        return nombreVaisseau;
    }

    public void setNombreVaisseau(int _nombreVaisseau)
    {
        this.nombreVaisseau = _nombreVaisseau;
    }

    public int getNombreIA()
    {
        return nombreIA;
    }
    public void setElements(string element)
    {
        this.element = element;
    }

    public void setNombreIA(int _nombreIA)
    {
        this.nombreIA = _nombreIA;
    }

    public int NombreVaisseau()
    {
        return MyAirships.Count;
    }
    public int NombreVaisseauHumain()
    {
        return MyAirshipsHumain.Count;
    }
    public int NombreCheckpoint(int range)
    {
        return MyAirships[range].GetComponent<EnemyController>().NombreCheckPoint();
    }
    public int NombreCheckpointHumain(int range)
    {
        return MyAirshipsHumain[range].GetComponent<PlayerController>().NombreCheckPoint();
    }
    public float RemainingDistance(int range)
    {

        return MyAirships[range].GetComponent<EnemyController>().RemainingDis();
    }
    public float RemainingDistanceHumain(int range)
    {

        return MyAirshipsHumain[range].GetComponent<PlayerController>().RemainingDis();
    }
    public int PosIA(int range)
    {
        return MyAirships[range].GetComponent<EnemyController>().Position();
    }
    public int PosHumain(int range)
    {
        return MyAirshipsHumain[range].GetComponent<PlayerController>().PositionHumain();
    }
    public void setTypeCourse(string typeCourse)
    {
        this.typeCourse = typeCourse;
    }

    public void clearGameController()
    {
        temps = 0;
        nombreVaisseau = 8;
        nombreIA = 7;
        typeCourse = "";
        CourseActuelDuChampionnat = 1;
        MyAirships = new List<GameObject>();
        MyAirshipsHumain = new List<GameObject>();
        ParticipantChampionnat = new List<Vaisseau>();
        for (int i = 0; i < 8; i++)
        {
            PointDuChampionnat[i] = 0;
        }
    }

}


   

