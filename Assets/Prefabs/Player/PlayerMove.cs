using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
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

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();

        shoot_audio = bullet_spawn.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        _direction_x = Input.GetAxis("Horizontal");
        _direction_z = Input.GetAxis("Vertical");
        doublespeed = Input.GetKey(KeyCode.LeftShift);

        _fire = _fire || Input.GetMouseButtonDown(0);

        if (Input.GetKeyDown(KeyCode.F))
        {
            spot_light.SetActive(!spot_light.activeSelf);
            spot_light_audio.Play();
        }
    }

    void Fire()
    {
        _fire = false;
        Instantiate(bullet_prefab, bullet_spawn.transform.position, bullet_spawn.transform.rotation);
        shoot_audio.Play();
    }

    private void FixedUpdate()
    {
        Vector3 direction = new Vector3(_direction_x, 0, _direction_z);

        _rb.AddRelativeForce(direction * speed);
        if (_rb.velocity.magnitude > 1 * (doublespeed ? 3 : 1)) _rb.velocity = _rb.velocity.normalized * (doublespeed ? 3 : 1);

        if (_fire) Fire();
    }
}
