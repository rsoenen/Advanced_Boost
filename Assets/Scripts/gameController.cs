using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class gameController : MonoBehaviour {

    private int nombreVaisseau=8;
    private int nombreIA=7;
    public Transform airshipEnemy;
    public Transform airshipAlly;

	// Use this for initialization
	void Start () {
        Track trackCircuit = GameObject.Find("Track").GetComponent<Track>();
        Quaternion rotate = Quaternion.Euler(0, 90, 0);

        for (int i = 1; i < nombreIA+1; i++){
            Vector3 pos = GameObject.Find("Spawn" + i).transform.position;
            Transform ennemy = (Transform)Instantiate(airshipEnemy, pos, rotate);
            ennemy.GetComponent<EnemyController>().track = trackCircuit;
            ennemy.tag = "VaisseauEnnemi"+i;
        }

        Text position = GameObject.Find("Position").GetComponent<Text>();
        Text tour = GameObject.Find("Tour").GetComponent<Text>();
        Text time = GameObject.Find("Time").GetComponent<Text>();
        Text info = GameObject.Find("Info").GetComponent<Text>();
        Text classement = GameObject.Find("Classement").GetComponent<Text>();


        for (int i = 1; i < nombreVaisseau - nombreIA + 1;i++){
            int numberSpawn = nombreIA + i;
            Vector3 pos = GameObject.Find("Spawn" + numberSpawn).transform.position;
            Transform player = (Transform)Instantiate(airshipAlly, pos, rotate);
            player.GetComponent<PlayerController>().track = trackCircuit;
            player.GetComponent<PlayerController>().lapText = tour;
            player.GetComponent<PlayerController>().timeElapsedText = time;
            player.GetComponent<PlayerController>().Position = position;
            player.GetComponent<PlayerController>().Info = info;
            player.GetComponent<PlayerController>().Classement = classement;
            player.tag = "Player" + i;
        }

	}

    // Update is called once per frame
    void Update()
    {
	
	}

    public int getNombreVaisseau(){
        return nombreVaisseau;
    }

    public void setNombreVaisseau(int _nombreVaisseau){
        this.nombreVaisseau=_nombreVaisseau;
    }

    public int getNombreIA() {
        return nombreIA;
    }

    public void setNombreIA(int _nombreIA){
        this.nombreIA = _nombreIA;
    }
}
