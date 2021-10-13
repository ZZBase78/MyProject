using UnityEngine;

public class Enemy : MonoBehaviour, IDamagable
{

    public Transform[] damage_points;

    Rigidbody _rb;

    bool next_destination_preset;
    Vector3 next_destination;

    bool moving;
    bool rotating;

    bool player_found;

    public float speed = 50;
    
    float time_to_attack;

    public void SetExplosionDamage(Vector3 form_position, Vector3 to_position, float damage)
    {
        SetDamage(form_position, to_position, damage);
    }
    public void SetDamage(Vector3 from_position, Vector3 to_position, float damage)
    {
        //foreach(Transform damage_point in damage_points)
        //{
        //    if (damage_point == null) continue; // не ясно, как может выполянтся метод setdamage если нет damage_point, видимо объект был уничтожен, как тогда выполняется скрипт
        //    Vector3 _dir = from_position - damage_point.position;
        //    if (Physics.Raycast(damage_point.position, _dir, out RaycastHit hitInfo, _dir.magnitude))
        //    {
        //        if (Mathf.Abs(hitInfo.distance - _dir.magnitude) <= 0.20f)
        //        {
        //            Destroy(gameObject); return;
        //        }
        //    }
        //}
        Destroy(gameObject);
    }

    private void Awake()
    {
        time_to_attack = 0;
        player_found = false;
        _rb = GetComponent<Rigidbody>();
        _rb.mass = Random.Range(50f, 80f);
        moving = false;
        rotating = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        next_destination_preset = false;
    }

    void TryNextDesination()
    {
        MapPoint current_mappoint = World.GetMapPosition(transform.position);
        MapRoom current_room = current_mappoint.mapRoom;

        int x = Random.Range(0, Settings.MapWidth);
        int y = Random.Range(0, Settings.MapHeight);

        if (Global.map[x, y].mapRoom == current_room)
        {
            next_destination = World.GetCellPosition(x, y) + new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));

            next_destination_preset = true;
        }

    }

    void MoveToNextDestination()
    {
        //Проверим угол к цели
        float angle = Vector3.Angle(next_destination - transform.position, transform.forward);

        if (angle > 3f)
        {
            //поворачиваемся к цели
            _rb.MoveRotation(Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(next_destination - transform.position), Time.fixedDeltaTime * 2));

            moving = false;
            rotating = true;
        }
        else
        {
            //двигаемся к цели
            _rb.velocity = transform.forward * Time.fixedDeltaTime * speed;

            moving = true;
            rotating = false;
        }

        if ((next_destination - transform.position).magnitude <= 0.1f)
        {
            next_destination_preset = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (time_to_attack > 0) time_to_attack -= Time.deltaTime;
        if (!next_destination_preset && !player_found)
        {
            TryNextDesination();
        }
    }

    void TryToPlayerDestination()
    {
        bool new_player_found = false;
        MapPoint current_mappoint = World.GetMapPosition(transform.position);
        MapRoom current_room = current_mappoint.mapRoom;

        if (Global.player != null)
        {
            float minx = current_room.x1 * Settings.CellWidth - 1f;
            float maxx = current_room.x2 * Settings.CellWidth + 1f;
            float miny = current_room.y1 * Settings.CellHeight - 1f;
            float maxy = current_room.y2 * Settings.CellHeight + 1f;

            if (Global.IsInterval(Global.player.transform.position.x, minx, maxx) && Global.IsInterval(Global.player.transform.position.z, miny, maxy))
            {
                //игрок внутри комнаты
                next_destination = Global.player.transform.position;
                next_destination_preset = true;
                new_player_found = true;
            }
        }

        if (new_player_found)
        {
            player_found = true;
            Vector3 destination = Global.player.transform.position - transform.position;
            if (destination.magnitude < 1f)
            {
                next_destination = transform.position; // останавливаемся
                next_destination_preset = false;
                if (time_to_attack <= 0)
                {
                    Global.player_script.SetDamage(transform.position, Global.player.transform.position, 1f);
                    time_to_attack = 1f;
                    World.PlayClip(Global.player.transform, 4);
                }
            }
                
        }
        else
        {
            if (player_found) 
            {
                player_found = false;
                next_destination_preset = false;
            }
        }

    }

    private void FixedUpdate()
    {
        if (next_destination_preset)
        {
            MoveToNextDestination();
        }
        TryToPlayerDestination();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.GetComponent<Enemy>() != null)
        //{
        //    //_rb.AddRelativeForce((transform.position - collision.gameObject.transform.position) * 5000);
        //    //_rb.AddRelativeForce(Vector3.left * 500);
        //    //_rb.MovePosition(-Vector3.forward);
        //    _rb.AddRelativeForce(Vector3.forward * -50);
        //    next_destination_preset = false;
        //}

        if (collision.gameObject.tag.Equals("Bullet")) Destroy(gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag.Equals("Enemy") && moving && !rotating)
        {
            next_destination_preset = false;
            moving = false;
            rotating = false;
        }
    }

    private void OnDestroy()
    {
        Global.enemies.Remove(this);
        if (Global.enemies.Count == 0)
        {
            if (Global.player != null) 
            {
                World.PlayClip(Global.player.transform, 5); // враги кончились
                Global.SelAllLampOn();
            } 
        }
        Destroy(this);
    }
}
