using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapRoom
{

    public int room_id = 0;

    public List<MapDoor> mapDoors;

    public int x1;
    public int y1;
    public int x2;
    public int y2;

    public RoomInterior interior;

    public MapRoom(int x1, int y1, int x2, int y2)
    {
        this.x1 = x1;
        this.y1 = y1;
        this.x2 = x2;
        this.y2 = y2;

        mapDoors = new List<MapDoor>();
    }

    public bool IsMaxSize()
    {
        int size = Mathf.Abs(x1 - x2) * Mathf.Abs(y1 - y2);
        return size >= Settings.MaxSizeRoom;
    }

    MapDoor IsDoorUp(int x, int y)
    {
        foreach(MapDoor mapDoor in mapDoors)
        {
            if (mapDoor.x == x && mapDoor.y == y && mapDoor.direction_door == 0) return mapDoor;
        }
        return null;
    }
    MapDoor IsDoorDown(int x, int y)
    {
        foreach (MapDoor mapDoor in mapDoors)
        {
            if (mapDoor.x == x && mapDoor.y == y && mapDoor.direction_door == 2) return mapDoor;
        }
        return null;
    }
    MapDoor IsDoorRight(int x, int y)
    {
        foreach (MapDoor mapDoor in mapDoors)
        {
            if (mapDoor.x == x && mapDoor.y == y && mapDoor.direction_door == 1) return mapDoor;
        }
        return null;
    }
    MapDoor IsDoorLeft(int x, int y)
    {
        foreach (MapDoor mapDoor in mapDoors)
        {
            if (mapDoor.x == x && mapDoor.y == y && mapDoor.direction_door == 3) return mapDoor;
        }
        return null;
    }

    public void Instantiate(GameObject level)
    {
        if (interior == null)
        {
            interior = RoomInteriors.GetRandom();
        }

        bool is_start_room = (this == Global.startRoom);

        GameObject _temp_go;

        GameObject wall = Global.prefabs[0]; // wall
        //UpWall
        for (int x = x1; x <= x2; x++)
        {
            MapDoor mapDoorUp = IsDoorUp(x, y2);
            if (mapDoorUp != null)
            {
                mapDoorUp.Instantiate(level, interior);
            }
            else if (!is_start_room)
            {
                _temp_go = GameObject.Instantiate(wall, World.GetCellUpCenterPosition(x, y2), World.UpQuaternion(), level.transform);
                _temp_go.GetComponent<WallUp>().texture_id = interior.wall;
            }
            
        }
            
        //DownWall
        for (int x = x1; x <= x2; x++)
        {
            MapDoor mapDoorDown = IsDoorDown(x, y1);
            if (mapDoorDown != null)
            {
                mapDoorDown.Instantiate(level, interior);
            }
            else if (!is_start_room)
            {
                _temp_go = GameObject.Instantiate(wall, World.GetCellDownCenterPosition(x, y1), World.DownQuaternion(), level.transform);
                _temp_go.GetComponent<WallUp>().texture_id = interior.wall;
            }
        }
        
        //LeftWall
        for (int y = y1; y <= y2; y++)
        {
            MapDoor mapDoorLeft = IsDoorLeft(x1, y);
            if (mapDoorLeft != null)
            {
                mapDoorLeft.Instantiate(level, interior);
            }
            else if (!is_start_room)
            {
                _temp_go = GameObject.Instantiate(wall, World.GetCellLeftCenterPosition(x1, y), World.LeftQuaternion(), level.transform);
                _temp_go.GetComponent<WallUp>().texture_id = interior.wall;
            }
        }
        
        //RightWall
        for (int y = y1; y <= y2; y++)
        {
            MapDoor mapDoorRight = IsDoorRight(x2, y);
            if (mapDoorRight != null)
            {
                mapDoorRight.Instantiate(level, interior);
            }
            else if (!is_start_room)
            {
                _temp_go = GameObject.Instantiate(wall, World.GetCellRightCenterPosition(x2, y), World.RightQuaternion(), level.transform);
                _temp_go.GetComponent<WallUp>().texture_id = interior.wall;
            }
        }

        if (!is_start_room)
        {
            GameObject floor = Global.prefabs[2]; // floor
            GameObject go = GameObject.Instantiate(floor, new Vector3((((float)x1 + x2) / 2) * Settings.CellWidth, 0, (((float)y1 + y2) / 2) * Settings.CellHeight), Quaternion.identity, level.transform);
            Floor floor_copm = go.GetComponent<Floor>();
            floor_copm.floor.transform.localScale = new Vector3((Mathf.Abs(x1 - x2) + 1) * Settings.CellWidth, floor_copm.floor.transform.localScale.y, (Mathf.Abs(y1 - y2) + 1) * Settings.CellHeight);
            floor_copm.floor_txture_id = interior.floor;

            GameObject roof = Global.prefabs[5]; // roof
            go = GameObject.Instantiate(roof, new Vector3((((float)x1 + x2) / 2) * Settings.CellWidth, 3, (((float)y1 + y2) / 2) * Settings.CellHeight), Quaternion.identity, level.transform);
            Roof roof_copm = go.GetComponent<Roof>();
            roof_copm.roof.transform.localScale = new Vector3((Mathf.Abs(x1 - x2) + 1) * Settings.CellWidth, floor_copm.floor.transform.localScale.y, (Mathf.Abs(y1 - y2) + 1) * Settings.CellHeight);
            roof_copm.roof_txture_id = interior.floor;
        }

        for (int x = x1; x <= x2; x++)
        {
            for (int y = y1; y <= y2; y++)
            {
                if ( (!is_start_room) && Random.Range(0,3) <= 5) // пока все присутствуют
                {
                    GameObject lamp = Global.prefabs[6]; // lamp
                    Vector3 lamp_position = World.GetCellPosition(x, y);
                    lamp_position.y = 3; // высота потолка

                    GameObject new_lamp = GameObject.Instantiate(lamp, lamp_position, Quaternion.Euler(180, 0, 0), level.transform);
                    Global.lamps.Add(new_lamp);

                }

            }
        }
    }
}
