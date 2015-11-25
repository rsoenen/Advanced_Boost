using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RaceController : MonoBehaviour {

    private int nombreia = 1;
    private GameObject Vaisseau;
    public List<GameObject> MyAirships = new List<GameObject>(); 
    // Use this for initialization
    void Start () {
	    for(int i=1;i< (nombreia+1);i++)
        {
            string tag = "VaisseauEnnemi" + i.ToString();
            Vaisseau = GameObject.FindWithTag(tag);
            MyAirships.Add(Vaisseau);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public int NombreVaisseau()
    {
        return MyAirships.Count;
    }
    public int NombreCheckpoint(int range)
    {

        return MyAirships[range].GetComponent<EnemyController>().NombreCheckPoint();
    }
    public int Distance(int range)
    {

        return MyAirships[range].GetComponent<EnemyController>().Remaining();
    }
    public Vector3 Pos(int range)
    {

        return MyAirships[range].GetComponent<EnemyController>().Position();
    }
    public Vector3 Actual(int range)
    {

        return MyAirships[range].GetComponent<EnemyController>().ActualCheck();
    }
}
