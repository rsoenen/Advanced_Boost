using UnityEngine;
using System.Collections;


public class EnemyController : VehicleController{

    NavMeshAgent agent;
    ArrayList listTarget;
    int index;
    int Checkpoint;
	// Use this for initialization
	void Start () {
        index = 0;
        Checkpoint = 0;
        agent = GetComponent<NavMeshAgent>();
        listTarget = new ArrayList();

       

        
        // Définition des cibles successives
        listTarget.Add(track.checkpoint1.transform.position);
        listTarget.Add(track.checkpoint2.transform.position);
        listTarget.Add(track.checkpoint3.transform.position);
        listTarget.Add(track.finish.transform.position);

        moveToNextTarget();

    }
    public int NombreCheckPoint()
    {
        return Checkpoint;
    }
    public int Remaining()
    {
        return (int)agent.remainingDistance;
    }
    public Vector3 Position()
    {
        return transform.position;
    }
    public Vector3 ActualCheck()
    {
        if (index> 0)
            {
                return (Vector3)listTarget[index - 1];
            }
        else
        {
            return (Vector3)listTarget[3];
        }
    }
    // Update is called once per frame
    void Update () {
        if (agent.remainingDistance < 5)
        {
            Checkpoint++;
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
