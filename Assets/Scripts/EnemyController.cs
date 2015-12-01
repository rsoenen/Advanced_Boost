using UnityEngine;
using System.Collections;


public class EnemyController : VehicleController{

    NavMeshAgent agent;
    ArrayList listTarget;
    private NavMeshPath path;
    private float elapsed = 0.0f;
    int index;
    int Checkpoint;
    float DistanceIA;
    bool activate = false;
    private float elapsedPos = 0.0f;
    private int ActualPos;
    private float MyTime;
    private GameController gameController;
    // Use this for initialization
    void Start ()
    {
        MyTime = Time.time;
        elapsedPos = 0.0f;
        ActualPos = 1;
        elapsed = 0.0f;
        path = new NavMeshPath();
        index = 0;
        Checkpoint = 0;
        agent = GetComponent<NavMeshAgent>();
        listTarget = new ArrayList();

        DistanceIA = 0;

        GameObject gameControllerObject = GameObject.FindWithTag("gameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        // Définition des cibles successives
        listTarget.Add(track.checkpoint1.transform.position);
        listTarget.Add(track.checkpoint2.transform.position);
        listTarget.Add(track.checkpoint3.transform.position);
        listTarget.Add(track.finish.transform.position);

        

    }
    public int Position()
    {
        return ActualPos;
    }
    public int NombreCheckPoint()
    {
        return Checkpoint;
    }
    public int Remaining()
    {
        return (int)agent.remainingDistance;
    }
    public float RemainingDis()
    {
        return DistanceIA;
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
        if (MyTime + 5 < Time.time )
        {
            if (!activate)
            {
                moveToNextTarget();
                activate = true;
            }
            else
            {
                elapsed += Time.deltaTime;
                if (elapsed > 0.05f)
                {
                    elapsed -= 0.05f;
                    if (index > 0)
                        NavMesh.CalculatePath(transform.position, (Vector3)listTarget[index - 1], NavMesh.AllAreas, path);
                    else
                        NavMesh.CalculatePath(transform.position, (Vector3)listTarget[3], NavMesh.AllAreas, path);
                }
                DistanceIA = PathLength(path);
                if (agent.remainingDistance < 5 && GetTime() + 10 < Time.time)
                {
                    moveToNextTarget();
                    Checkpoint++;
                }
                elapsedPos += Time.deltaTime;
                if (elapsedPos > 0.2f && Checkpoint<12)
                {
                    elapsedPos -= 0.2f;
                    ActualPos = 1;
                    for (int i = 0; i < gameController.NombreVaisseau(); i++)
                    {
                        int check = gameController.NombreCheckpoint(i);
                        if (check > Checkpoint)
                            ActualPos++;
                        else if (check == Checkpoint)
                        {
                            if (DistanceIA > gameController.RemainingDistance(i))
                                ActualPos++;
                        }
                    }
                    for (int i = 0; i < gameController.NombreVaisseauHumain(); i++)
                    {
                        if (gameController.MyAirshipsHumain[i].tag != tag)
                        {
                            int check = gameController.NombreCheckpointHumain(i);
                            
                            if (check > Checkpoint)
                                ActualPos++;
                            else if (check == Checkpoint)
                            {
                                if (DistanceIA > gameController.RemainingDistanceHumain(i))
                                    ActualPos++;
                            }
                        }
                    }
                }
            }
        }
	}

    void moveToNextTarget()
    {
        agent.SetDestination((Vector3)listTarget[index]);
        index++;
        if (index >= listTarget.Count)
            index = 0;
    }
    float PathLength(NavMeshPath path)
    {
        if (path.corners.Length < 2)
            return 0;

        Vector3 previousCorner = path.corners[0];
        float lengthSoFar = 0.0F;
        int i = 1;
        while (i < path.corners.Length)
        {
            Vector3 currentCorner = path.corners[i];
            lengthSoFar += Vector3.Distance(previousCorner, currentCorner);
            previousCorner = currentCorner;
            i++;
        }
        return lengthSoFar;
    }
}
