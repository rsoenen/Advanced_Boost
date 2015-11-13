using UnityEngine;
using System.Collections;

public class EnemyController : VehicleController{

    NavMeshAgent agent;

    ArrayList listTarget;
    int index;

	// Use this for initialization
	void Start () {
        index = 0;
        agent = GetComponent<NavMeshAgent>();
        listTarget = new ArrayList();


        // Définition des cibles successives
        listTarget.Add(new Vector3(50, 0.05f, -17));
        listTarget.Add(new Vector3(17, 0.05f, -71));
        listTarget.Add(new Vector3(-32, 0.05f, -75));
        listTarget.Add(new Vector3(-12, 0.05f, 0));

        moveToNextTarget();

	}
	
	// Update is called once per frame
	void Update () {
        if (agent.remainingDistance < 50)
        {
            moveToNextTarget();
        }
	}

    void moveToNextTarget()
    {
        agent.SetDestination((Vector3)listTarget[index]);
        index++;
        if (index >= listTarget.Count)
            index = 0;
    }
}
