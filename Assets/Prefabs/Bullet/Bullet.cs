using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    Rigidbody _rb;
    AudioSource _audio;
    public AudioClip[] _audio_clip;

    bool _audio_was_played;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.velocity = transform.forward * 100;

        _audio = GetComponent<AudioSource>();

        _audio_was_played = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, 3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Floor") && !_audio_was_played)
        {
            _audio_was_played = true;
            _audio.clip = _audio_clip[Random.Range(0, _audio_clip.Length)];
            _audio.Play();
        }
    }
}
