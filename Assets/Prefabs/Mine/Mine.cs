using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{

    public enum Status { Off, Safe, PrepareToArm, Armed, PrepareToBoom }

    AudioSource _audio;
    public AudioSource _audio2;
    public AudioClip clip_prepareToArm;
    public AudioClip clip_prepareToBoom;
    public AudioClip clip_BompArmed;
    public AudioClip clip_BombFall;

    public Light spot1;
    public Light spot2;
    public Light spot3;
    public Light spot4;

    Status status;

    float spot_timer;
    float status_timer;

    Color[] prepared_colors;
    int current_color;

    public MineDamageDetector mineDamageDetector;

    public void ChangeStatus(Status new_status)
    {
        Status old = status;
        status = new_status;
        if (status == Status.Off)
        {
            spot1.enabled = false;
            spot2.enabled = false;
            spot3.enabled = false;
            spot4.enabled = false;
            status_timer = 0;
            spot_timer = 0;
        }
        else if (status == Status.Safe)
        {
            spot1.enabled = true;
            spot2.enabled = true;
            spot3.enabled = true;
            spot4.enabled = true;
            spot1.color = Color.green;
            spot2.color = Color.green;
            spot3.color = Color.green;
            spot4.color = Color.green;
            status_timer = 0;
            spot_timer = 0;
        }
        else if (status == Status.PrepareToArm)
        {
            spot1.enabled = true;
            spot2.enabled = true;
            spot3.enabled = true;
            spot4.enabled = true;
            spot1.color = Color.yellow;
            spot2.color = Color.yellow;
            spot3.color = Color.yellow;
            spot4.color = Color.yellow;
            status_timer = 3f;
            spot_timer = 0.5f;
            current_color = 0;
            prepared_colors = new Color[2];
            prepared_colors[0] = Color.yellow;
            prepared_colors[1] = Color.black;

            _audio.clip = clip_prepareToArm;
            _audio.Play();
        }
        else if (status == Status.Armed)
        {
            spot1.enabled = true;
            spot2.enabled = true;
            spot3.enabled = true;
            spot4.enabled = true;
            spot1.color = Color.red;
            spot2.color = Color.red;
            spot3.color = Color.red;
            spot4.color = Color.red;
            status_timer = 0;
            spot_timer = 0;
            current_color = 0;
        }
        else if (status == Status.PrepareToBoom)
        {
            spot1.enabled = true;
            spot2.enabled = true;
            spot3.enabled = true;
            spot4.enabled = true;
            spot1.color = Color.red;
            spot2.color = Color.red;
            spot3.color = Color.red;
            spot4.color = Color.red;
            status_timer = 2f;
            spot_timer = 0.1f;
            current_color = 0;
            prepared_colors = new Color[2];
            prepared_colors[0] = Color.red;
            prepared_colors[1] = Color.black;

            _audio.clip = clip_prepareToBoom;
            _audio.Play();
        }
    }

    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if ((status == Status.Off) || (status == Status.Safe) || (status == Status.Armed))
        {
            //status_timer не меняем
            //spot_timer не меняем
        }
        else if (status == Status.PrepareToArm)
        {
            status_timer -= Time.deltaTime;
            spot_timer -= Time.deltaTime;
            if (spot_timer <= 0)
            {
                current_color = (current_color + 1) % prepared_colors.Length;
                spot1.color = prepared_colors[current_color];
                spot2.color = prepared_colors[current_color];
                spot3.color = prepared_colors[current_color];
                spot4.color = prepared_colors[current_color];
                spot_timer = 0.5f;
                if (current_color == 0)
                {
                    _audio.clip = clip_prepareToArm;
                    _audio.Play();
                }
            }
            if (status_timer <= 0)
            {
                _audio.clip = clip_BompArmed;
                _audio.Play();

                ChangeStatus(Status.Armed);
            }
        }
        else if (status == Status.PrepareToBoom)
        {
            status_timer -= Time.deltaTime;
            spot_timer -= Time.deltaTime;
            if (spot_timer <= 0)
            {
                current_color = (current_color + 1) % prepared_colors.Length;
                spot1.color = prepared_colors[current_color];
                spot2.color = prepared_colors[current_color];
                spot3.color = prepared_colors[current_color];
                spot4.color = prepared_colors[current_color];
                spot_timer = 0.1f;
            }
            if (status_timer <= 0)
            {
                ChangeStatus(Status.Off);
                Boom();
            }
        }


        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    ChangeStatus(Status.Off);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    ChangeStatus(Status.Safe);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha3))
        //{
        //    ChangeStatus(Status.PrepareToArm);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha4))
        //{
        //    ChangeStatus(Status.PrepareToBoom);
        //}
    }

    void Boom()
    {
        Instantiate(Global.prefabs[9], transform.position, Quaternion.identity); //Boom audio


        foreach (IDamagable i in mineDamageDetector.list)
        {
            if (i != null && transform != null) i.SetDamage(transform.position, Vector3.zero, 1000);
        }

        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            if (!_audio2.isPlaying)
            {
                _audio2.clip = clip_BombFall;
                _audio2.Play();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if ((status == Status.Armed) && (other.CompareTag("Enemy") || other.CompareTag("Player")))
        {
            ChangeStatus(Status.PrepareToBoom);
        }
    }
}
