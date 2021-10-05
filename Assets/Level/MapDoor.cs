using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDoor
{
    public int door_id = 0;

    public bool open;

    public MapDoor coonecting_door;

    public DoorUp door_up;

    public int x;
    public int y;

    public int direction_door; // 0 - up, 1 - right, 2 - down, 3 - left

    public void SetOpen(bool new_open)
    {
        open = new_open;
        if (coonecting_door != null) coonecting_door.open = new_open;
    }
    public MapDoor(int x, int y, int direction)
    {
        this.x = x;
        this.y = y;
        this.direction_door = direction;
        this.open = false;
    }

    public static void GetDoorsPairLeftRight(int x1, int x2, int y, int index, out MapDoor mapDoor1, out MapDoor mapDoor2)
    {
        mapDoor1 = new MapDoor(x1, y, 1);
        mapDoor2 = new MapDoor(x2, y, 3);
        mapDoor1.door_id = index;
        mapDoor2.door_id = index;
        mapDoor1.coonecting_door = mapDoor2;
        mapDoor2.coonecting_door = mapDoor1;

        Global.mapDoors.Add(mapDoor1);
        Global.mapDoors.Add(mapDoor2);

        if (mapDoor1.x >= 0 && mapDoor1.x < Settings.MapWidth && mapDoor1.y >= 0 && mapDoor1.y < Settings.MapHeight)
        {
            Global.map[mapDoor1.x, mapDoor1.y].door_right = mapDoor1;
        }
        if (mapDoor2.x >= 0 && mapDoor2.x < Settings.MapWidth && mapDoor2.y >= 0 && mapDoor2.y < Settings.MapHeight)
        {
            Global.map[mapDoor2.x, mapDoor2.y].door_left = mapDoor2;
        }
    }
    public static void GetDoorsPairUpDown(int x, int y1, int y2, int index, out MapDoor mapDoor1, out MapDoor mapDoor2)
    {
        mapDoor1 = new MapDoor(x, y1, 2);
        mapDoor2 = new MapDoor(x, y2, 0);
        mapDoor1.door_id = index;
        mapDoor2.door_id = index;
        mapDoor1.coonecting_door = mapDoor2;
        mapDoor2.coonecting_door = mapDoor1;

        Global.mapDoors.Add(mapDoor1);
        Global.mapDoors.Add(mapDoor2);

        if (mapDoor1.x >= 0 && mapDoor1.x < Settings.MapWidth && mapDoor1.y >= 0 && mapDoor1.y < Settings.MapHeight)
        {
            Global.map[mapDoor1.x, mapDoor1.y].door_down = mapDoor1;
        }
        if (mapDoor2.x >= 0 && mapDoor2.x < Settings.MapWidth && mapDoor2.y >= 0 && mapDoor2.y < Settings.MapHeight)
        {
            Global.map[mapDoor2.x, mapDoor2.y].door_up = mapDoor2;
        }
    }

    public void Instantiate(GameObject level, RoomInterior interior)
    {
        GameObject door = Global.prefabs[1]; // door

        GameObject go = null;

        if (direction_door == 0)
            go = GameObject.Instantiate(door, World.GetCellUpCenterPosition(x, y), World.UpQuaternion(), level.transform);
        if (direction_door == 2)
            go = GameObject.Instantiate(door, World.GetCellDownCenterPosition(x, y), World.DownQuaternion(), level.transform);
        if (direction_door == 1)
            go = GameObject.Instantiate(door, World.GetCellRightCenterPosition(x, y), World.RightQuaternion(), level.transform);
        if (direction_door == 3)
            go = GameObject.Instantiate(door, World.GetCellLeftCenterPosition(x, y), World.LeftQuaternion(), level.transform);

        if (go != null)
        {
            door_up = go.GetComponentInChildren<DoorUp>();
            door_up.wall_texture_id = interior.wall;
            door_up.door_texture_id = interior.door;

            door_up.map_door = this;
        }
        
        
    }

}
