using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy2 : MonoBehaviour, IDamagable
{

    int maxhealth = 255;
    int currenthealth;

    public MeshRenderer capsula_renderer;

    public Vector3[] patrolpoint;

    public int indexpatrolpoint;

    NavMeshAgent agent;

    //Rigidbody _rb;

    public void SetDamage(Vector3 form_position, Vector3 to_position, float damage)
    {
        //Vector3 direction = (to_position - form_position).normalized;
        //_rb.AddForce(direction * damage, ForceMode.Impulse);
        currenthealth = currenthealth - 200;
        if (currenthealth < 0) currenthealth = 0;
        capsula_renderer.material.color = new Color(((float)maxhealth - currenthealth) / 255, (float)currenthealth / 255, (float)currenthealth / 255);
        if (currenthealth == 0)
        {
            Global.enemy2Spawner.Destroy_Enemy(gameObject);
            Global.Enemy2_killed++;
            Destroy(this);
        }
    }

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
        currenthealth = maxhealth;
        agent = GetComponent<NavMeshAgent>();
        //_rb = GetComponent<Rigidbody>();
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
