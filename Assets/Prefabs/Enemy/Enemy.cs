using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamagable
{

    public Transform[] damage_points;

    Rigidbody _rb;

    bool next_destination_preset;
    Vector3 next_destination;

    bool moving;
    bool rotating;

    public float speed = 50;

    public void SetDamage(Vector3 from_position, float damage)
    {
        foreach(Transform damage_point in damage_points)
        {
            if (damage_point == null) continue; // не €сно, как может выпол€нтс€ метод setdamage если нет damage_point, видимо объект был уничтожен, как тогда выполн€етс€ скрипт
            Vector3 _dir = from_position - damage_point.position;
            if (Physics.Raycast(damage_point.position, _dir, out RaycastHit hitInfo, _dir.magnitude))
            {
                if (Mathf.Abs(hitInfo.distance - _dir.magnitude) <= 0.20f)
                {
                    Destroy(gameObject); return;
                }
            }
        }
    }

    private void Awake()
    {
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
        //ѕроверим угол к цели
        float angle = Vector3.Angle(next_destination - transform.position, transform.forward);

        if (angle > 3f)
        {
            //поворачиваемс€ к цели
            _rb.MoveRotation(Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(next_destination - transform.position), Time.fixedDeltaTime * 2));

            moving = false;
            rotating = true;
        }
        else
        {
            //двигаемс€ к цели
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
        if (!next_destination_preset)
        {
            TryNextDesination();
        }
    }

    private void FixedUpdate()
    {
        if (next_destination_preset)
        {
            MoveToNextDestination();
        }
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
        Destroy(this);
    }
}
