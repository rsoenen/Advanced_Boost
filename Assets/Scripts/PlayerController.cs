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

		//on avance selon la quantité de mouvement à effectuer
		transform.position += transform.forward * forwardMove * Time.deltaTime;

		//on met des bordures pour éviter au vaisseau de sortir du background
		GetComponent<Rigidbody>().position = new Vector3(
			Mathf.Clamp(GetComponent<Rigidbody>().position.x,boundary.xMin,boundary.xMax),
			0.0f,
			Mathf.Clamp(GetComponent<Rigidbody>().position.z,boundary.zMin,boundary.zMax)
			);





	}
}