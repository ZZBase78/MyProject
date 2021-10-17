using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour, IDamagable
{

    Rigidbody _rb;

    public float speed = 50;

    public bool doublespeed = false;

    float _direction_x;
    float _direction_z;

    AudioSource shoot_audio;

    bool _fire;

    public GameObject bullet_spawn;

    public GameObject bullet_prefab;

    public GameObject spot_light;

    public AudioSource spot_light_audio;

    public Transform mine_spawn;

    bool _mine;

    bool _jump;
    float jumpforce;

    public int health;

    public GameObject main_camera;

    Camera maincamera_component;

    bool drob_bust;
    bool drob_bust_sound1_played;
    bool drob_bust_sound2_played;
    GameObject tiktak;

    void EndDrobBust()
    {
        if (tiktak != null) Destroy(tiktak);
        drob_bust = false;
        if (!drob_bust_sound2_played)
        {
            drob_bust_sound2_played = true;
            World.PlayClip(transform, 7); // damn
        }
    }
    public void Drob_pickup()
    {
        drob_bust = true;
        if (!drob_bust_sound1_played)
        {
            drob_bust_sound1_played = true;
            World.PlayClip(transform, 6); // i like big gun
        }
        tiktak = new GameObject();
        tiktak.transform.position = transform.position;
        tiktak.transform.parent = transform;
        World.PlayClip(tiktak.transform, 9); // tiktak
        Invoke("EndDrobBust", 10f);
    }

    public void TurnCamera(bool value)
    {
        main_camera.GetComponent<Camera>().enabled = value;
        //main_camera.GetComponent<AudioListener>().enabled = value;
        main_camera.GetComponent<CrossFire>().enabled = value;
    }

    public void SetExplosionDamage(Vector3 form_position, Vector3 to_position, float damage)
    {
        SetDamage(form_position, to_position, damage);
    }
    public void SetDamage(Vector3 from_position, Vector3 to_position, float damage)
    {
        health = health - (int)damage;
    }

    private void Awake()
    {
        drob_bust_sound1_played = false;
        drob_bust = false;
        health = 1000;
        Global.player = gameObject;
        Global.player_script = this;

        maincamera_component = main_camera.GetComponent<Camera>();

    }
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();

        shoot_audio = bullet_spawn.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    Global.SelAllLampOn();
        //}

        _direction_x = Input.GetAxis("Horizontal");
        _direction_z = Input.GetAxis("Vertical");
        doublespeed = Input.GetKey(KeyCode.LeftShift);

        if (Input.GetMouseButtonDown(0) && maincamera_component.enabled)
        {
            _fire = true;
        }
        //_fire = _fire || Input.GetMouseButtonDown(0);

        if (Input.GetKeyDown(KeyCode.F))
        {
            spot_light.SetActive(!spot_light.activeSelf);
            spot_light_audio.Play();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            _mine = true;
        }
        if (Input.GetKeyDown(KeyCode.Space) && !_jump)
        {
            _jump = true;
            jumpforce = 3;
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            bool current = main_camera.GetComponent<Camera>().enabled;
            TurnCamera(!current);
            if (current) Global.camera_3d.TurnOn(); else Global.camera_3d.TurnOff();
        }
    }

    void Fire()
    {
        _fire = false;
        //Instantiate(bullet_prefab, bullet_spawn.transform.position, bullet_spawn.transform.rotation);
        if (!drob_bust)
        {
            shoot_audio.clip = Global.clips[11];
            shoot_audio.Play();
        }
        else
        {
            shoot_audio.clip = Global.clips[8];
            shoot_audio.Play();
        }
        

        if (Physics.Raycast(bullet_spawn.transform.position, bullet_spawn.transform.forward, out RaycastHit hitinfo, 100f))
        {
            if (!hitinfo.collider.CompareTag("NoDamage") || drob_bust)
            {
                IDamagable i = hitinfo.transform.GetComponentInParent<IDamagable>();
                if (i != null)
                {
                    //Debug.Log("Попал" + hitinfo.transform);
                    if (drob_bust)
                    {
                        i.SetDamage(bullet_spawn.transform.position, hitinfo.point, 500f);
                    }
                    else
                    {
                        i.SetDamage(bullet_spawn.transform.position, hitinfo.point, 200f);
                    }
                    
                }
                else
                {
                    //Debug.Log("Не Попал");
                }
            }
            else
            {
                World.PlayClip(transform, 10);
            }
        }
    }
    void Mine()
    {
        _mine = false;
        GameObject go = Instantiate(Global.prefabs[8], mine_spawn.position, Quaternion.identity);
        Rigidbody _go_rb = go.GetComponent<Rigidbody>();
        Mine _go_mine = go.GetComponent<Mine>();
        _go_mine.ChangeStatus(global::Mine.Status.PrepareToArm);
        _go_rb.AddForce(mine_spawn.forward * 100);
    }

    void Jump()
    {
        jumpforce -= Time.fixedDeltaTime * 4f;
        if (jumpforce <= 0)
        {
            _jump = false;
        }
    }

    private void FixedUpdate()
    {
        //Vector3 direction = new Vector3(_direction_x, 0, _direction_z);

        //_rb.AddRelativeForce(direction * speed);
        //if (_rb.velocity.magnitude > 1 * (doublespeed ? 3 : 1)) _rb.velocity = _rb.velocity.normalized * (doublespeed ? 3 : 1);
        Vector3 direction = new Vector3(_direction_x, 0, _direction_z) * (doublespeed ? 3 : 1);
        direction += Vector3.up * jumpforce;
        transform.Translate(direction * 0.05f);
        //transform.position = transform.position + direction * 0.05f;

        if (_fire) Fire();
        if (_mine) Mine();
        if (_jump) Jump();
    }
}
