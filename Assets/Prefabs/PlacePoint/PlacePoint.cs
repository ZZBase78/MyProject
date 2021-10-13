using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacePoint : MonoBehaviour
{

    Renderer _r;
    float y;

    bool playerIn;
    bool cubeIn;

    private void Awake()
    {
        playerIn = false;
        cubeIn = false;
        _r = GetComponent<Renderer>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        y = Mathf.Repeat(y + Time.deltaTime, 10f);
        _r.material.mainTextureOffset = new Vector2(0f, -y);

        //if (playerIn && !Global.startRoom.mapDoors[0].door_up.open)
        //{
        //    Global.startRoom.mapDoors[0].door_up.SetOpen(true, true);
        //    Global.startRoom.mapDoors[0].coonecting_door.door_up.SetOpen(true, true);
        //} else if (!playerIn && Global.startRoom.mapDoors[0].door_up.open)
        //{
        //    Global.startRoom.mapDoors[0].door_up.SetOpen(false, true);
        //    Global.startRoom.mapDoors[0].coonecting_door.door_up.SetOpen(false, true);
        //}
        if (playerIn || cubeIn)
        {
            Global.startRoom.mapDoors[0].door_up.SetOpen(true, true);
            Global.startRoom.mapDoors[0].coonecting_door.door_up.SetOpen(true, true);
        }
        else 
        {
            Global.startRoom.mapDoors[0].door_up.SetOpen(false, true);
            Global.startRoom.mapDoors[0].coonecting_door.door_up.SetOpen(false, true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerIn = true;
        }
        if (other.tag == "PointCube")
        {
            cubeIn = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerIn = false;
        }
        if (other.tag == "PointCube")
        {
            cubeIn = false;
        }
    }
}
