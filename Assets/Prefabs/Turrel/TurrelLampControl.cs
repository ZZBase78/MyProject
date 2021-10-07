using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurrelLampControl : MonoBehaviour
{

    public GameObject[] lamps;

    public Material[] materials;

    public AudioSource[] audios;

    public AudioClip[] clips;

    public int status; //0 - выключены, 1 - горит, 2 - мигает

    int flash1;
    int flash2;
    int currentflash;
    float starttimeflash1;
    float starttimeflash2;
    float currenttimeflash1;
    float currenttimeflash2;
    float startdurationflashing;
    float currentdurationflashing;
    int endflash;

    public void SetColor(int index, bool changestatus)
    {
        foreach (GameObject go in lamps)
        {
            if (go == null) continue;
            go.GetComponent<Renderer>().material = materials[index];
        }
        if (changestatus)
        {
            if (index == 0) status = 0; else status = 1;
        }
    }

    public void SetFlashing(int _flash1, int _flash2, float _starttimeflash1, float _starttimeflash2, float _startdurationflashing, int _endflash)
    {
        status = 2;
        flash1 = _flash1;
        flash2 = _flash2;
        currentflash = flash1;
        starttimeflash1 = _starttimeflash1;
        starttimeflash2 = _starttimeflash2;
        currenttimeflash1 = _starttimeflash1;
        currenttimeflash2 = _starttimeflash2;
        startdurationflashing = _startdurationflashing;
        currentdurationflashing = _startdurationflashing;
        endflash = _endflash;

        SetColor(currentflash, false);
        World.PlayClip(audios[0], clips[0]);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    void TestControl()
    {
        if (Input.GetKey(KeyCode.Alpha0)) SetFlashing(2, 0, 0.1f, 0.1f, 1f, 0);
        if (Input.GetKey(KeyCode.Alpha1)) SetColor(1, true);
        if (Input.GetKey(KeyCode.Alpha2)) SetColor(2, true);
        if (Input.GetKey(KeyCode.Alpha3)) SetColor(3, true);
    }

    void Status2Control()
    {
        if (currentflash == flash1)
        {
            currenttimeflash1 -= Time.deltaTime;
            if (currenttimeflash1 <= 0)
            {
                currentflash = flash2;
                currenttimeflash2 = starttimeflash2;
                SetColor(currentflash, false);
            }
        }
        else
        {
            currenttimeflash2 -= Time.deltaTime;
            if (currenttimeflash2 <= 0)
            {
                currentflash = flash1;
                currenttimeflash1 = starttimeflash1;
                SetColor(currentflash, false);
                World.PlayClip(audios[0], clips[0]);
            }
        }

        currentdurationflashing -= Time.deltaTime;
        if (currentdurationflashing <= 0)
        {
            SetColor(endflash, true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //TestControl();

        if (status == 2)
        {
            Status2Control();
        }
    }

}
