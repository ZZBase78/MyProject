using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurrelFireControl : MonoBehaviour
{

    public GameObject smallGun;
    public Light pointlight;

    public GameObject startposition;
    public GameObject endposition;

    float lightduation = 0.1f;
    float currentligh1duration;

    float gun_interpolate_pos;

    public AudioSource _audio;
    public AudioClip _clip;

    public void Fire(Vector3 target)
    {
        if (smallGun.transform.position == startposition.transform.position)
        {
            pointlight.enabled = true;
            currentligh1duration = lightduation;
            World.PlayClip(_audio, _clip);
            smallGun.transform.position = endposition.transform.position;
            gun_interpolate_pos = 0;
        }
    }

    private void Update()
    {
        if (pointlight.enabled)
        {
            currentligh1duration -= Time.deltaTime;
            if (currentligh1duration <= 0) pointlight.enabled = false;
        }

        if (smallGun.transform.position != startposition.transform.position)
        {
            gun_interpolate_pos += Time.deltaTime;
            smallGun.transform.position = Vector3.Lerp(endposition.transform.position, startposition.transform.position, 3f * gun_interpolate_pos);
        }
    }
}
