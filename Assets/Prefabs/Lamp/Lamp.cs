using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : MonoBehaviour
{

    public GameObject go_light;
    public GameObject go_spotlight;

    float time_to_change;

    AudioSource player;

    public int mode; //0 - –аботает посто€нно, 1 - работает с перебо€ми, 2 - в основном не работает, 3 - Ќе работает

    // Start is called before the first frame update
    void Start()
    {

        player = GetComponent<AudioSource>();

        int moderange = Random.Range(0, 31);

        mode = 0;

        if (Global.IsInterval(moderange, 0, 0))
        {
            mode = 0;
        } 
        else if (Global.IsInterval(moderange, 1, 2))
        {
            mode = 1;
        }
        else if (Global.IsInterval(moderange, 3, 5))
        {
            mode = 2;
        }
        else if (Global.IsInterval(moderange, 6, 30))
        {
            mode = 3;
        }

        if ((mode == 0) || (mode == 1))
        {
            go_light.SetActive(true);
            go_spotlight.SetActive(true);
        }
        if ((mode == 2) || (mode == 3))
        {
            go_light.SetActive(false);
            go_spotlight.SetActive(false);
        }

        SetTimeToChange(go_light.activeSelf);
    }

    void SetTimeToChange(bool onoff)
    {
        if (mode == 1)
        {
            if (onoff)
            {
                time_to_change = Random.Range(1f, 5f);
            }
            else
            {
                time_to_change = Random.Range(0f, 0.5f);
            }
        }else if (mode == 2)
        {
            if (onoff)
            {
                time_to_change = Random.Range(0f, 0.5f);
            }
            else
            {
                time_to_change = Random.Range(1f, 5f);
            }
        }
    }

    void ChangeLight()
    {
        go_light.SetActive(!go_light.activeSelf);
        go_spotlight.SetActive(go_light.activeSelf);

        if ((mode == 1) || (mode == 2)) SetTimeToChange(go_light.activeSelf);

        if (go_light.activeSelf) player.Play();

    }

    // Update is called once per frame
    void Update()
    {
        if ((mode == 1) || (mode == 2))
        {
            time_to_change -= Time.deltaTime;
            if (time_to_change <= 0) ChangeLight();
        }
    }
}
