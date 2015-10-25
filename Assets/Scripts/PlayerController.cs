using UnityEngine;
using System.Collections;

[System.Serializable]
public class Boundary{
	public float xMin,xMax,zMin,zMax;

}

public class PlayerController : MonoBehaviour {

	public float speed = 3;
	public float turnRate = 3;
	public Boundary boundary;


	//VARIABLE TURBO ELEMENTS MC
	public string elements;
	private int turboElement = 0;
	private const int maxTurboElement=100;

	void Start(){
		GameGUI.maxTurboElement = (int)maxTurboElement;
	}
	void FixedUpdate(){

		//on capte les input "avancer" et "tourner"
		float forwardMove = Input.GetAxis("Vertical") * speed;
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

		//on avance selon la quantité de mouvement à effectuer
		transform.position += transform.forward * forwardMove * Time.deltaTime;

		//on met des bordures pour éviter au vaisseau de sortir du background
		GetComponent<Rigidbody>().position = new Vector3(
			Mathf.Clamp(GetComponent<Rigidbody>().position.x,boundary.xMin,boundary.xMax),
			0.0f,
			Mathf.Clamp(GetComponent<Rigidbody>().position.z,boundary.zMin,boundary.zMax)
			);
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
}