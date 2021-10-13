using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointCube : MonoBehaviour
{

    Rigidbody _rb;
    AudioSource _audio;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _audio = GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_rb.velocity != Vector3.zero)
        {
            if (!_audio.isPlaying) _audio.Play();
            _audio.volume = 1;
        }
        else
        {
            _audio.Stop();
            _audio.volume = 0;
        }
    }
}
