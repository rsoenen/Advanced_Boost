using UnityEngine;
using System.Collections;

public class ReboundScript : MonoBehaviour {

    public float height;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        Rigidbody rigidBody = GetComponent<Rigidbody>();
        Vector3 heightVector = new Vector3(rigidBody.position.x, height, rigidBody.position.z);
        rigidBody.position = heightVector;


	}
}
