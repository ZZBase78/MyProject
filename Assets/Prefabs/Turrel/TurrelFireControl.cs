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

    public GameObject gun_point;

    public void Fire(Vector3 target)
    {
        if (smallGun.transform.position == startposition.transform.position)
        {
            pointlight.enabled = true;
            currentligh1duration = lightduation;
            World.PlayClip(_audio, _clip);
            smallGun.transform.position = endposition.transform.position;
            gun_interpolate_pos = 0;

            bool obstacle_found = false;
            Vector3 player_direction = Global.player.transform.position - gun_point.transform.position;
            RaycastHit[] hits = Physics.RaycastAll(gun_point.transform.position, gun_point.transform.forward, player_direction.magnitude);
            foreach (RaycastHit hit in hits)
            {
                if (hit.transform.tag == "Enemy") continue;
                if (hit.transform.tag == "Player") continue;
                if (hit.transform.tag == "Turrel") continue;
                obstacle_found = true;
                break;
            }
            if (!obstacle_found)
            {
                Global.player_script.SetDamage(gun_point.transform.position, Global.player.transform.position, 1f);
            }
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
