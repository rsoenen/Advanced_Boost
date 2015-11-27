using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RaceController : MonoBehaviour {

    private int nombreia = 1;
    private int nombrehumain = 1;
    private GameObject Vaisseau;
    public List<GameObject> MyAirships = new List<GameObject>();
    public List<GameObject> MyAirshipsHumain = new List<GameObject>();
    // Use this for initialization
    void Start ()
    {
        for (int i = 1; i < (nombreia + 1); i++)
        {
            string tag = "VaisseauEnnemi" + i.ToString();
            Vaisseau = GameObject.FindWithTag(tag);
            MyAirships.Add(Vaisseau);
        }
        for (int i = 1; i < (nombrehumain + 1); i++)
        {
            string tag = "Player" + i.ToString();
            Vaisseau = GameObject.FindWithTag(tag);
            MyAirshipsHumain.Add(Vaisseau);
        }
    }
	
	// Update is called once per frame
	void Update () {

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
}
