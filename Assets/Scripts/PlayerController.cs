using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : VehicleController{

    void Start()
    {
        GameGUI.maxTurboElement = (int)maxTurboElement;
    }

    void FixedUpdate()
    {
        // Inputs
        forwardMove = Input.GetAxis("Vertical");
        turnForce = Input.GetAxis("Horizontal") * turnRate;

        // Turbo !!!!
        if (Input.GetKey("space"))
        {
            UseTurbo();
            GameGUI.turboElement = (int)turboElement;
        }

        TurnVehicle();
        Forward();

        UpdateCollisionTime();

    }

    // Texte Collision
    public Text collisionText;
    void SetCollisionText(int value, int level)
    {
        collisionText.text = "Distance (in m) since last collision : " + value.ToString();
    }
    void UpdateCollisionTime()
    {
        float dist = Time.deltaTime * speed;

        distance += dist;
        int value = (int)(distance * ptByKm / 1000);
        SetCollisionText(value, Level(value));
    }


}