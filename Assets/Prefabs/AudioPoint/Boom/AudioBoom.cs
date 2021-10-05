using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioBoom : MonoBehaviour
{

    AudioSource _audio;

    public AudioClip clip;

    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _audio.Play();
        Destroy(gameObject, 10f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
