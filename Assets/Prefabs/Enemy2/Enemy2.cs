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

    bool move_to_player; // признак что идет преследования игрока

    Rigidbody _rb;

    bool waitingInvokeToEndMoveToPlayer; // признак ожидания выполнения метода Invoke, для отключения преследования игрока

    Animator _anim;

    void AfterExplosion()
    {
        if (this.ToString() == "null") return;
        _rb.isKinematic = true; //включаем кинематику
        agent.enabled = true; // включаем агента
    }

    public void SetExplosionDamage(Vector3 form_position, Vector3 to_position, float damage)
    {
        _rb.isKinematic = false; // отключаем кинематику для реализации отдачи от взрыва
        agent.enabled = false; //отключае агента
        Vector3 explosion_direction = (pointToExplosion.transform.position - form_position).normalized;
        _rb.AddForce(explosion_direction * 300f);
        Invoke("AfterExplosion", 2f); //установим кинематику, т.к. иначе мешает передвижению агента

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

        if (Global.player == null) return; // игрока нет на сцене
        Vector3 destination = Global.player.transform.position - transform.position;
        if (destination.magnitude > 10f) return; // расстояние до игрока более 10м
        float angle = Vector3.Angle(transform.forward, destination);
        if ((angle > 90) && (destination.magnitude > 3f)) return; //игрок вне угла видимости если расстояние более 3м, при меньшем расстоянии угол зрения не иммет значения

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
                ////Удар игрока
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
            // путь до игрока найден
            agent.SetDestination(navigate_to_player.transform.position);
            move_to_player = true; // устанавливаем признак что начинаем преследовать игрока
            _anim.SetBool("Armed", true);
        }
                
    }

    void EndMoveToPlayer()
    {
        waitingInvokeToEndMoveToPlayer = false; // снимаем признак ожидания метода
        move_to_player = false; // останавливаем преследование
        _anim.SetBool("Armed", false);
    }

    // Update is called once per frame
    void Update()
    {
        if (time_to_damage > 0) time_to_damage -= Time.deltaTime;

        if (agent.enabled && (agent.remainingDistance <= agent.stoppingDistance))
        {
            // достигли точки маршрута

            if (move_to_player) 
            {
                //преследовали игрока
                if (navigate_to_player == null)
                {
                    //игрок потерян из виду, достигли последнюю известную точку игрока
                    if (!waitingInvokeToEndMoveToPlayer) // проверка чтобы каждый раз не запускать Invoke
                    {
                        waitingInvokeToEndMoveToPlayer = true;
                        Invoke("EndMoveToPlayer", 3f); // прекратим преследование через 3 сек
                    }

                }
                else
                {
                    Vector3 diretion = navigate_to_player.transform.position - transform.position;
                    Quaternion q_direction = Quaternion.LookRotation(diretion);
                    //игрок виден, Достигли игрока, нужно контролировать только поворот в сторону игрока
                    transform.localRotation = Quaternion.RotateTowards(transform.rotation, q_direction, 90f * Time.deltaTime);
                }
            }
            else
            {
                //патрулируем
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
