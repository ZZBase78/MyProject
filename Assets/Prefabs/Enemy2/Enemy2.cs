using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy2 : MonoBehaviour
{

    public Vector3[] patrolpoint;

    public int indexpatrolpoint;

    NavMeshAgent agent;

    public Vector3 GetRandomPoint()
    {
        //Зона патруля
        float x = Random.Range(0f, 30f);
        float z = Random.Range(-60f, -3f);
        float y = 10f;
        return new Vector3(x, y, z);
    }

    public Vector3 GetGroundPoint(Vector3 point)
    {
        if (Physics.Raycast(point, Vector3.down, out RaycastHit hitinio, 10f)){
            return hitinio.point;
        }
        else
        {
            return Vector3.zero;
        }
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    // Start is called before the first frame update
    void Start()
    {
        patrolpoint = new Vector3[5];
        for (int i = 0; i < patrolpoint.Length; i++) 
        {
            do
            {
                Vector3 groundpoint = GetGroundPoint(GetRandomPoint());
                bool pathfound = agent.CalculatePath(groundpoint, new NavMeshPath());
                if (pathfound)
                {
                    patrolpoint[i] = groundpoint;
                    break;
                }
            } while (true);
                
        }
        indexpatrolpoint = 0;
        agent.SetDestination(patrolpoint[indexpatrolpoint]);
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            indexpatrolpoint = (indexpatrolpoint + 1) % patrolpoint.Length;
            agent.SetDestination(patrolpoint[indexpatrolpoint]);
        }
    }
}
