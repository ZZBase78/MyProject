using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy2 : MonoBehaviour, IDamagable
{

    int maxhealth = 255;
    int currenthealth;

    public GameObject pointToExplosion;

    public MeshRenderer capsula_renderer;

    public Vector3[] patrolpoint;

    public int indexpatrolpoint;

    NavMeshAgent agent;

    GameObject navigate_to_player;

    float time_to_damage;

    bool move_to_player; // ������� ��� ���� ������������� ������

    Rigidbody _rb;

    bool waitingInvokeToEndMoveToPlayer; // ������� �������� ���������� ������ Invoke, ��� ���������� ������������� ������

    Animator _anim;

    void AfterExplosion()
    {
        if (this.ToString() == "null") return;
        _rb.isKinematic = true; //�������� ����������
        agent.enabled = true; // �������� ������
    }

    public void SetExplosionDamage(Vector3 form_position, Vector3 to_position, float damage)
    {
        _rb.isKinematic = false; // ��������� ���������� ��� ���������� ������ �� ������
        agent.enabled = false; //�������� ������
        Vector3 explosion_direction = (pointToExplosion.transform.position - form_position).normalized;
        _rb.AddForce(explosion_direction * 300f);
        Invoke("AfterExplosion", 2f); //��������� ����������, �.�. ����� ������ ������������ ������

        SetDamage(form_position, to_position, damage);
    }

    public void SetDamage(Vector3 form_position, Vector3 to_position, float damage)
    {
        if (gameObject == null) return;
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
        //���� �������
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
        move_to_player = false;
        time_to_damage = 0;
        currenthealth = maxhealth;
        agent = GetComponent<NavMeshAgent>();
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
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

    public void SetDamageToPlayer()
    {
        Vector3 destination = Global.player.transform.position - transform.position;
        if (destination.magnitude <= 2f)
        {
            Global.player_script.SetDamage(transform.position, Vector3.zero, 1);
            time_to_damage = 1;
            World.PlayClip(Global.player.transform, 4);
        }

    }

    void FindPlayer()
    {

        navigate_to_player = null;

        if (Global.player == null) return; // ������ ��� �� �����
        Vector3 destination = Global.player.transform.position - transform.position;
        if (destination.magnitude > 10f) return; // ���������� �� ������ ����� 10�
        float angle = Vector3.Angle(transform.forward, destination);
        if ((angle > 90) && (destination.magnitude > 3f)) return; //����� ��� ���� ��������� ���� ���������� ����� 3�, ��� ������� ���������� ���� ������ �� ����� ��������

        bool obstacle_found = false;
        RaycastHit[] hits = Physics.RaycastAll(transform.position, destination, destination.magnitude);
        foreach(RaycastHit hit in hits)
        {
            if (hit.transform.tag == "Enemy") continue;
            if (hit.transform.tag == "Player") continue;
            obstacle_found = true;
            break;
        }

        if (!obstacle_found)
        {
            navigate_to_player = Global.player;
            if (destination.magnitude <= 2f)
            {
                _anim.SetTrigger("Attack");
                ////���� ������
                //if (time_to_damage <= 0)
                //{
                //    Global.player_script.SetDamage(transform.position, Vector3.zero, 1);
                //    time_to_damage = 1;
                //    World.PlayClip(Global.player.transform, 4);
                //}
                
            }
        }

    }

    void NavigateToPlayer()
    {
        if (navigate_to_player == null)
        {
            return;
        }
        if (!agent.enabled) return;

        bool pathfound = agent.CalculatePath(navigate_to_player.transform.position, new NavMeshPath());
        if (pathfound)
        {
            // ���� �� ������ ������
            agent.SetDestination(navigate_to_player.transform.position);
            move_to_player = true; // ������������� ������� ��� �������� ������������ ������
            _anim.SetBool("Armed", true);
        }
                
    }

    void EndMoveToPlayer()
    {
        waitingInvokeToEndMoveToPlayer = false; // ������� ������� �������� ������
        move_to_player = false; // ������������� �������������
        _anim.SetBool("Armed", false);
    }

    // Update is called once per frame
    void Update()
    {
        if (time_to_damage > 0) time_to_damage -= Time.deltaTime;

        if (agent.enabled && (agent.remainingDistance <= agent.stoppingDistance))
        {
            // �������� ����� ��������

            if (move_to_player) 
            {
                //������������ ������
                if (navigate_to_player == null)
                {
                    //����� ������� �� ����, �������� ��������� ��������� ����� ������
                    if (!waitingInvokeToEndMoveToPlayer) // �������� ����� ������ ��� �� ��������� Invoke
                    {
                        waitingInvokeToEndMoveToPlayer = true;
                        Invoke("EndMoveToPlayer", 3f); // ��������� ������������� ����� 3 ���
                    }

                }
                else
                {
                    Vector3 diretion = navigate_to_player.transform.position - transform.position;
                    Quaternion q_direction = Quaternion.LookRotation(diretion);
                    //����� �����, �������� ������, ����� �������������� ������ ������� � ������� ������
                    transform.localRotation = Quaternion.RotateTowards(transform.rotation, q_direction, 90f * Time.deltaTime);
                }
            }
            else
            {
                //�����������
                indexpatrolpoint = (indexpatrolpoint + 1) % patrolpoint.Length;
                agent.SetDestination(patrolpoint[indexpatrolpoint]);
            }
        }

        FindPlayer();

        NavigateToPlayer();
    }

    private void OnDestroy()
    {
        Destroy(this);
    }
}
