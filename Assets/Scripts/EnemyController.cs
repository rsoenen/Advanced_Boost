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
        listTarget.Add(track.checkpoint1.transform.position);
        listTarget.Add(track.checkpoint2.transform.position);
        listTarget.Add(track.checkpoint3.transform.position);
        listTarget.Add(track.finish.transform.position);

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
