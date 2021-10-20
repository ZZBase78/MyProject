using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorUp : MonoBehaviour
{

    public MapDoor map_door;

    public GameObject center;
    public GameObject left;
    public GameObject right;
    public GameObject leftslide;
    public GameObject rightslide;

    public int wall_texture_id;
    public int door_texture_id;

    bool _open;
    public bool open;

    public AudioClip audioClip_open;
    public AudioClip audioClip_close;

    AudioSource _audio_source;


    float time_to_autoclose;

    public void SetOpen(bool new_open, bool playaudio)
    {
        if (_open == open)
        {
            open = new_open;
        }

        if (playaudio) _audio_source.volume = 1; else _audio_source.volume = 0;

        time_to_autoclose = 5;
    }

    public void ChangeOpen(bool playaudio)
    {
        if (_open == open)
        {
            open = !open;
        }

        if (playaudio) _audio_source.volume = 1; else _audio_source.volume = 0;

        time_to_autoclose = 5;
    }

    private void Awake()
    {
        _audio_source = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _open = false;
        open = false;
        

        World.SetTextureXY(center, wall_texture_id);
        World.SetTextureXY(left, wall_texture_id);
        World.SetTextureXY(right, wall_texture_id);

        World.SetTextureXY(leftslide, door_texture_id);
        World.SetTextureXY(rightslide, door_texture_id);
    }

    void Slide()
    {
        if (open)
        {
            leftslide.transform.Translate(Vector3.left * Time.deltaTime * 0.5f);
            rightslide.transform.Translate(Vector3.right * Time.deltaTime * 0.5f);

            if ((leftslide.transform.position - rightslide.transform.position).magnitude >= 2.1)
            {
                _open = open;
            }

            if (!_audio_source.isPlaying || _audio_source.clip != audioClip_open)
            {
                _audio_source.clip = audioClip_open;
                _audio_source.Play();
            }
        }
        else
        {
            leftslide.transform.Translate(Vector3.right * Time.deltaTime * 0.5f);
            rightslide.transform.Translate(Vector3.left * Time.deltaTime * 0.5f);
            if ((leftslide.transform.position - rightslide.transform.position).magnitude <= 0.9)
            {
                _open = open;
            }

            if (!_audio_source.isPlaying || _audio_source.clip != audioClip_close)
            {
                _audio_source.clip = audioClip_close;
                _audio_source.Play();
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (_open != open)
        {
            Slide();
        }

        if (time_to_autoclose > 0) time_to_autoclose -= Time.deltaTime;

        if (time_to_autoclose <= 0 && open)
        {
            SetOpen(false, true);
            map_door.coonecting_door.door_up.SetOpen(false, false);
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag.Equals("Player") || other.tag.Equals("Enemy"))
        {
            time_to_autoclose = 5;
        }
    }
}
