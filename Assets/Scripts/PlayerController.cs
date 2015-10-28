﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[System.Serializable]
public class Boundary{
	public float xMin,xMax,zMin,zMax;

}

public class PlayerController : MonoBehaviour {

	public float acceleration = 3;
    public float maxSpeed = 5;
	public float turnRate = 3;
	public Boundary boundary;

    private float speed;


	//VARIABLE TURBO ELEMENTS MC
	public string elements;
	private int turboElement = 0;
	private const int maxTurboElement=100;

	void Start(){
        acceleration *= 100;

		GameGUI.maxTurboElement = (int)maxTurboElement;

        distance = 0;
	}
	void FixedUpdate(){

		//on capte les input "avancer" et "tourner"
		float forwardMove = Input.GetAxis("Vertical");
		float turnForce = Input.GetAxis("Horizontal") * turnRate;

		//si le joueur va vers l'avant le sens de rotation est standard
		if(forwardMove >= 0){
			transform.Rotate(0,turnForce,0);
		}
		//s'il va en arrière, on inverse le sens
		else{
			transform.Rotate(0,-turnForce,0);
		}


		//On ajoute le turbo élementaire si le joueur appuie sur espace
		if (Input.GetKey ("space")&&turboElement>0) {
			turboElement--;
			forwardMove=forwardMove+2;
			GameGUI.turboElement = (int)turboElement;
		}

        /*  CODE THIBAUT */
		//on avance selon la quantité de mouvement à effectuer
		//transform.position += transform.forward * forwardMove * Time.deltaTime;

		// on met des bordures pour éviter au vaisseau de sortir du background 
        /*
		GetComponent<Rigidbody>().position = new Vector3(
			Mathf.Clamp(GetComponent<Rigidbody>().position.x,boundary.xMin,boundary.xMax),
			0.0f,
			Mathf.Clamp(GetComponent<Rigidbody>().position.z,boundary.zMin,boundary.zMax)
			);
        */

        /*  CODE QUENTIN */
        speed = GetComponent<Rigidbody>().velocity.magnitude;
        if (speed < maxSpeed)
        {
            Vector3 force = new Vector3(0.0f, 0.0f, forwardMove) * acceleration; // Le -1 sert a inverser le sens (bug ?)
            GetComponent<Rigidbody>().AddRelativeForce(force, ForceMode.Acceleration);
        }

        UpdateCollisionTime();
	}

	//GESTION RECHARGEMENT TURBO ELEMENT
	void OnTriggerStay(Collider other) {
		if (other.CompareTag ("Feu")) {
			if (elements=="Feu"&&maxTurboElement>turboElement){
				turboElement+=2;
			}
			if (elements=="Eau"&&0<turboElement){
				turboElement--;
			}
		} else if (other.CompareTag ("Eau")) {
			if (elements=="Eau"&&maxTurboElement>turboElement){
				turboElement+=2;
			}
			if (elements=="Feu"&&0<turboElement){
				turboElement--;
			}
		} else if (other.CompareTag ("Lumiere")) {
			if (elements=="Feu"&&maxTurboElement>turboElement){
				turboElement++;
			}
			if (elements=="Lumiere"&&maxTurboElement>turboElement){
				turboElement+=2;
			}
		} else if(other.CompareTag ("Tenebre")){
			if (elements=="Eau"&&maxTurboElement>turboElement){
				turboElement++;
			}
			if (elements=="Tenebre"&&maxTurboElement>turboElement){
				turboElement+=2;
			}
		} 
		GameGUI.turboElement = (int)turboElement;
	}

    // Gestion des collisions
    public Text collisionText;
    public float ptByKm;
    private float distance;

    void OnCollisionEnter(Collision myCollisionInfo)
    {
        distance = 0;
    }

    void UpdateCollisionTime()
    {
        float dist = Time.deltaTime * speed;

        distance += dist;
        int value = (int)(distance * ptByKm / 1000);

        SetCollisionText(value, Level(value));
    }

    void SetCollisionText(int value, int level)
    {
        collisionText.text = "Distance (in m) since last collision : " + value.ToString();
    }

    int Level(int val)
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
    int Boost(int level)
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