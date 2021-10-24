using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioVisualizer : MonoBehaviour
{

    public GameObject partical_prefab;

    public GameObject cube_prefab;

    public AudioSource _audio;

    public float speed_down_scale;

    //public float pow_a;

    public float sound_scale;

    GameObject[] cubes;
    int length = 64;
    int length_short = 16;

    float[] spectrum;
    float[] average;
    float[] maximum;
    float[] normalized;
    float[] cooldown;

    // Start is called before the first frame update
    void Start()
    {
        cubes = new GameObject[length_short];
        for (int i = 0; i< length_short; i++)
        {
            cubes[i] = Instantiate(cube_prefab, new Vector3(i, 0, 0), Quaternion.identity);
        }

        spectrum = new float[length];
        average = new float[length_short];
        maximum = new float[length_short];
        normalized = new float[length_short];
        cooldown = new float[length_short];
        for (int i = 0; i < length_short; i++)
        {
            maximum[i] = 0;
            cooldown[i] = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        

        _audio.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);

        for (int i = 0; i < length_short; i++)
        {
            int start = i * 4;
            int end = start + 4;
            average[i] = 0;
            for (int j = start; j < end; j++)
            {
                average[i] = average[i] + spectrum[j];
            }
            average[i] = average[i] / 4;
            if (average[i] > maximum[i]) maximum[i] = average[i];
            normalized[i] = average[i] / maximum[i];
        }

        for (int i = 0; i < length_short; i++)
        {
            maximum[i] = maximum[i] / (1f + 0.1f * Time.deltaTime);
        }

        for (int i = 0; i < length_short; i++)
        {

            if (cooldown[i] > 0) cooldown[i] -= Time.deltaTime;

            Vector3 scale = cubes[i].transform.localScale;

            //гашение
            scale.y = scale.y - Time.deltaTime * speed_down_scale;
            if (scale.y < 1) scale.y = 1;

            //всплеск
            //float y_scale = spectrum[i] * sound_scale * Mathf.Pow(pow_a, i);
            //float y_scale = spectrum[i] * sound_scale * pow_a * i * i;
            //float y_scale = spectrum[i] * sound_scale * Mathf.Pow((float)i * pow_a, 2);
            float y_scale = normalized[i] * sound_scale;
            if (y_scale > scale.y)
            {
                scale.y = y_scale;
                if (normalized[i] > 0.75f && (cooldown[i] <= 0f))
                {
                    GameObject partical_go = Instantiate(partical_prefab, new Vector3(i, scale.y, 0), partical_prefab.transform.rotation);
                    Color c = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));

                    ParticleSystem.MainModule main = partical_go.GetComponent<ParticleSystem>().main;
                    main.startColor = c;

                    Destroy(partical_go, 5f);
                    cooldown[i] = 0.1f;
                }
            }

            cubes[i].transform.localScale = new Vector3(scale.x, scale.y, scale.z);

            Vector3 newposition = cubes[i].transform.position;
            newposition.y = scale.y / 2;
            cubes[i].transform.position = newposition;

            float green = 1 - scale.y / sound_scale;
            float red = scale.y / sound_scale;
            float blue = 0;
            cubes[i].GetComponent<Renderer>().material.color = new Color(red, green, blue);
        }
    }
}
