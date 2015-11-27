using UnityEngine;
using System.Collections;

public class VehicleController : MonoBehaviour {

    public float acceleration = 3;
    public float maxSpeed = 5;
    public float turnRate = 3;

    protected float speed;

    //VARIABLE TURBO ELEMENTS MC
    public string elements;
    protected bool turboElementActif;
    protected int turboElement;
    protected const int maxTurboElement = 100;
    protected int valeurBoost;
    float currentTime = 0;

    protected float turnForce;
    protected float forwardMove;

    // Gestion CheckPoint
    protected int lap=1;
    public Track track;
    protected float timeM;


    void Start()
    {
        timeM = Time.time;
        distance = 0;
        turboElement = 0;
        turboElementActif = false;
        valeurBoost = 0;
        
    }
    public float GetTime()
    {
        return timeM;
    }
    protected void TurnVehicle()
    {
        //si le joueur va vers l'avant le sens de rotation est standard
        if (forwardMove >= 0)
            transform.Rotate(0, turnForce, 0);

        //s'il va en arrière, on inverse le sens
        else
            transform.Rotate(0, -turnForce, 0);
        
    }

    protected void UseTurbo()
    {
        //On ajoute le turbo élementaire si le joueur appuie sur espace
        if (turboElement > 0)
        {
            turboElement--;
            if (speed < maxSpeed)
            {
                Vector3 force = new Vector3(0.0f, 0.0f, forwardMove) * acceleration * 9;
                //GetComponent<Rigidbody>().AddRelativeForce(force, ForceMode.Acceleration);
            }            
        }
    }

    protected void Forward()
    {
        speed = GetComponent<Rigidbody>().velocity.magnitude;
        valeurBoost = 0;
        if (turboElementActif){
            valeurBoost = 2;
        }
        //float deltaTime=Time.time-currentTime;
        
        //currentTime = Time.time;
        if (speed < (maxSpeed + valeurBoost))
        {
            //Debug.Log("boost en cours");
            Vector3 force = new Vector3(0.0f, 0.0f, forwardMove) * acceleration * 100;
            GetComponent<Rigidbody>().AddRelativeForce(force, ForceMode.Acceleration);
        }
    }

    

    //GESTION RECHARGEMENT TURBO ELEMENT
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Feu"))
        {
            if (elements == "Feu" && maxTurboElement > turboElement)
            {
                turboElement += 2;
            }
            if (elements == "Eau" && 0 < turboElement)
            {
                turboElement--;
            }
        }
        else if (other.CompareTag("Eau"))
        {
            if (elements == "Eau" && maxTurboElement > turboElement)
            {
                turboElement += 2;
            }
            if (elements == "Feu" && 0 < turboElement)
            {
                turboElement--;
            }
        }
        else if (other.CompareTag("Lumiere"))
        {
            if (elements == "Feu" && maxTurboElement > turboElement)
            {
                turboElement++;
            }
            if (elements == "Lumiere" && maxTurboElement > turboElement)
            {
                turboElement += 2;
            }
        }
        else if (other.CompareTag("Tenebre"))
        {
            if (elements == "Eau" && maxTurboElement > turboElement)
            {
                turboElement++;
            }
            if (elements == "Tenebre" && maxTurboElement > turboElement)
            {
                turboElement += 2;
            }
        }
        GameGUI.turboElement = (int)turboElement;
    }

    // Gestion des collisions
    public float ptByKm;
    protected float distance;

    void OnCollisionEnter(Collision myCollisionInfo)
    {
        if (myCollisionInfo.gameObject.CompareTag("Mur"))
        {
            distance = 0;
        }

    }

    protected int Level(int val)
    {
        if (val < 0)
            return -1;
        else if (val >= 0 && val < 100)
            return 0;
        else if (val >= 100 && val < 300)
            return 1;
        else if (val >= 300 && val < 700)
            return 2;
        else if (val >= 700)
            return 3;
        else
            return -1;
    }
    protected int Boost(int level)
    {
        switch (level)
        {
            case 1:
                return 5;
            case 2:
                return 15;
            case 3:
                return 30;
            default:
                return 0;
        }
    }

}
