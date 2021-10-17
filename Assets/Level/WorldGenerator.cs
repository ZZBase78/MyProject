using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MyRoot
{

    public GameObject wallUp;

    public GameObject level;

    void NewMap()
    {
        Global.map = new MapPoint[Settings.MapWidth, Settings.MapHeight];

        for (int x = 0; x < Settings.MapWidth; x++)
        {
            for (int y = 0; y < Settings.MapWidth; y++)
            {
                Global.map[x, y] = new MapPoint(x, y);
            }
        }
    }

    MapPoint GetRandom(bool first)
    {
        int x = 0;
        int y = 0;
        int new_room = Random.Range(0, 30);
        if (first || Global.mapRooms.Count < Settings.MinRoomCount) new_room = 0;
    
        if (new_room == 0)
        {
            do
            {
                x = Random.Range(0, Settings.MapWidth);
                y = Random.Range(0, Settings.MapHeight);
            } while (Global.map[x, y].mapRoom != null);
        }
        else
        {
            do
            {
                x = Random.Range(0, Settings.MapWidth);
                y = Random.Range(0, Settings.MapHeight);
            } while (Global.map[x, y].mapRoom == null);

        }
        return Global.map[x, y];
    }

    bool IsFreeRect(int x1, int y1, int x2, int y2)
    {
        for (int x = x1; x<=x2; x++)
        {
            if (x < 0 || x >= Settings.MapWidth) return false;
            for (int y = y1; y <= y2; y++)
            {
                if (y < 0 || y >= Settings.MapHeight) return false;
                if (Global.map[x, y].mapRoom != null) return false;
            }
        }
        return true;
    }

    void SetRoomRect(int x1, int y1, int x2, int y2, MapRoom mapRoom, ref int freetotal)
    {
        for (int x = x1; x <= x2; x++)
        {
            for (int y = y1; y <= y2; y++)
            {
                MapPoint mapPoint = Global.map[x, y];
                mapPoint.mapRoom = mapRoom;
                freetotal--;
            }
        }
        mapRoom.x1 = Mathf.Min(mapRoom.x1, x1);
        mapRoom.y1 = Mathf.Min(mapRoom.y1, y1);
        mapRoom.x2 = Mathf.Max(mapRoom.x2, x2);
        mapRoom.y2 = Mathf.Max(mapRoom.y2, y2);
    }

    // Start is called before the first frame update
    void Start()
    {
        Global.lamps = new List<GameObject>();



        Global.Enemy2_killed = 0;

        Global.mapTurrels = new List<MapTurrel>();
        Global.mapRooms = new List<MapRoom>();

        NewMap();

        int total = Settings.MapWidth * Settings.MapHeight;
        int freetotal = total;

        while (freetotal > 0)
        {

            //Debug.Log(freetotal);

            MapPoint mapPoint = GetRandom(freetotal == total);

            if (mapPoint.mapRoom == null)
            {
                // Создаем новую комнату размером 1х1
                MapRoom mapRoom = new MapRoom(mapPoint.x, mapPoint.y, mapPoint.x, mapPoint.y);
                mapPoint.mapRoom = mapRoom;
                freetotal--;
                Global.mapRooms.Add(mapRoom);
            }
            else
            {
                // пробуем расширить существующую
                MapRoom mapRoom = mapPoint.mapRoom;
                bool maxsize = mapRoom.IsMaxSize();
                bool can_left = !maxsize && IsFreeRect(mapRoom.x1 - 1, mapRoom.y1, mapRoom.x1 - 1, mapRoom.y2);
                bool can_right = !maxsize && IsFreeRect(mapRoom.x2 + 1, mapRoom.y1, mapRoom.x2 + 1, mapRoom.y2);
                bool can_up = !maxsize && IsFreeRect(mapRoom.x1, mapRoom.y1 - 1, mapRoom.x2, mapRoom.y1 - 1);
                bool can_down = !maxsize && IsFreeRect(mapRoom.x1, mapRoom.y2 + 1, mapRoom.x2, mapRoom.y2 + 1);
                if (can_left || can_right || can_up || can_down)
                {
                    int direction = 0;
                    while (direction == 0)
                    {
                        int new_direction = Random.Range(1, 5);
                        if (new_direction == 1 && can_left) direction = new_direction;
                        if (new_direction == 2 && can_right) direction = new_direction;
                        if (new_direction == 3 && can_up) direction = new_direction;
                        if (new_direction == 4 && can_down) direction = new_direction;
                    }
                    if (direction == 1) SetRoomRect(mapRoom.x1 - 1, mapRoom.y1, mapRoom.x1 - 1, mapRoom.y2, mapRoom, ref freetotal);
                    if (direction == 2) SetRoomRect(mapRoom.x2 + 1, mapRoom.y1, mapRoom.x2 + 1, mapRoom.y2, mapRoom, ref freetotal);
                    if (direction == 3) SetRoomRect(mapRoom.x1, mapRoom.y1 - 1, mapRoom.x2, mapRoom.y1 - 1, mapRoom, ref freetotal);
                    if (direction == 4) SetRoomRect(mapRoom.x1, mapRoom.y2 + 1, mapRoom.x2, mapRoom.y2 + 1, mapRoom, ref freetotal);
                }
            }
        }

        int start_x = Random.Range(0, Settings.MapWidth);
        int end_x;
        do
        {
            end_x = Random.Range(0, Settings.MapWidth);
        } while (end_x == start_x);

        Global.startRoom = new MapRoom(start_x, -1, start_x, -1);
        Global.endRoom = new MapRoom(end_x, Settings.MapHeight, end_x, Settings.MapHeight);

        Generate_Doors();

        InstantiateMap(level);

        Global.startRoom.Instantiate(level);
        Global.endRoom.Instantiate(level);

        foreach(MapKey mapKey in Global.mapKeys)
        {
            mapKey.Instantiate();
        }

        //MapTurrel mapTurrel = new MapTurrel();
        //mapTurrel.x = start_x;
        //mapTurrel.y = 0;
        //mapTurrel.Instantiate();

        for (int i = 0; i <= Global.mapRooms.Count; i++)
        {
            MapTurrel mapTurrel = new MapTurrel();
            if (mapTurrel.GenerateCoordinates())
            {
                Global.mapTurrels.Add(mapTurrel);
                mapTurrel.Instantiate();
            }
        }

        //prize
        GameObject.Instantiate(Global.prefabs[7], World.GetCellPosition(end_x, Settings.MapHeight), Quaternion.identity);

        GameObject.Instantiate(Global.prefabs[4]); // enemy spawn

        //GameObject.Instantiate(Global.prefabs[3], World.GetCellPosition(Global.startRoom.x1, Global.startRoom.y1), Quaternion.identity); // player
        GameObject.Instantiate(Global.prefabs[3], new Vector3(1f, 0, -60f), Quaternion.identity); // player

        GameObject.Instantiate(Global.prefabs[13]); // enemy 2 spawner

        GameObject.Instantiate(Global.prefabs[15]); // drob_spawner

    }

    void Generate_Doors()
    {
        Global.mapDoors = new List<MapDoor>();
        Global.mapKeys = new List<MapKey>();
        Global.collectKeys = new List<MapKey>();
        Global.usedKeys = new List<MapKey>();
        MapKey mapKey;

        int total_rooms = Global.mapRooms.Count;

        int index = 1; // счетчик индексов объектов дверей и комнат

        //Первая дверь на входе открывается по тригеру
        MapDoor.GetDoorsPairUpDown(Global.startRoom.x1, 0, -1, index, out MapDoor mapDoor1, out MapDoor mapDoor2);
        mapDoor1.open = false;
        mapDoor2.open = false;
        index++;
        Global.map[Global.startRoom.x1, 0].mapRoom.room_id = index;
        Global.map[Global.startRoom.x1, 0].mapRoom.mapDoors.Add(mapDoor1);
        Global.startRoom.mapDoors.Add(mapDoor2);
        total_rooms--;

        while (total_rooms > 0)
        {
            int direction;
            int x1;
            int y1;
            int x2;
            int y2;

            while (true)
            {
                direction = Random.Range(0, 2);
                if (direction == 0) //горизонтальная
                {
                    x1 = Random.Range(0, Settings.MapWidth - 1);
                    y1 = Random.Range(0, Settings.MapHeight);
                    x2 = x1 + 1;
                    y2 = y1;
                }
                else
                {
                    x1 = Random.Range(0, Settings.MapWidth);
                    y1 = Random.Range(0, Settings.MapHeight - 1);
                    x2 = x1;
                    y2 = y1 + 1;
                }
                if ((Global.map[x1, y1].mapRoom.room_id == 0) && (Global.map[x2, y2].mapRoom.room_id != 0))
                {
                    break;
                }
                if ((Global.map[x1, y1].mapRoom.room_id != 0) && (Global.map[x2, y2].mapRoom.room_id == 0))
                {
                    break;
                }
            }

            if (direction == 0)
            {
                MapDoor.GetDoorsPairLeftRight(x1, x2, y1, index, out mapDoor1, out mapDoor2);
            }
            else
            {
                MapDoor.GetDoorsPairUpDown(x1, y2, y1, index, out mapDoor1, out mapDoor2);
            }

            //Генерим ключ для данной двери
            mapKey = new MapKey();
            mapKey.Generate(index);
            Global.mapKeys.Add(mapKey);

            index++;
            if (Global.map[x1, y1].mapRoom.room_id == 0) Global.map[x1, y1].mapRoom.room_id = index;
            if (Global.map[x2, y2].mapRoom.room_id == 0) Global.map[x2, y2].mapRoom.room_id = index;
            Global.map[x1, y2].mapRoom.mapDoors.Add(mapDoor1);
            Global.map[x2, y1].mapRoom.mapDoors.Add(mapDoor2);
            total_rooms--;

        }


        //Дверь на выход
        MapDoor.GetDoorsPairUpDown(Global.endRoom.x1, Settings.MapHeight, Settings.MapHeight - 1, index, out mapDoor1, out mapDoor2);
        //Генерим ключ для данной двери
        mapKey = new MapKey();
        mapKey.Generate(index);
        Global.mapKeys.Add(mapKey);

        index++;
        Global.map[Global.endRoom.x1, Settings.MapHeight - 1].mapRoom.room_id = index;
        Global.map[Global.endRoom.x1, Settings.MapHeight - 1].mapRoom.mapDoors.Add(mapDoor2);
        Global.endRoom.mapDoors.Add(mapDoor1);
    }

    void InstantiateMap(GameObject level)
    {
        foreach (MapRoom mapRoom in Global.mapRooms)
        {
            mapRoom.Instantiate(level);
        }
    }
    void InstantiateMap1()
    {
        for (int x = 0; x < Settings.MapWidth; x++)
        {
            for (int y = 0; y < Settings.MapWidth; y++)
            {
                //Левая
                if (x == 0) Instantiate(wallUp, World.GetCellLeftCenterPosition(x, y), World.LeftQuaternion());
                //Правая
                if (x == Settings.MapWidth - 1)
                {
                    Instantiate(wallUp, World.GetCellRightCenterPosition(x, y), World.RightQuaternion());
                } else if ((x < Settings.MapWidth - 1) && (Global.map[x,y].mapRoom != Global.map[x+1, y].mapRoom))
                {
                    Instantiate(wallUp, World.GetCellRightCenterPosition(x, y), World.RightQuaternion());
                }
                //Нижняя
                if (y == 0) Instantiate(wallUp, World.GetCellDownCenterPosition(x, y), World.DownQuaternion());
                //Верхняя
                if (y == Settings.MapWidth - 1)
                {
                    Instantiate(wallUp, World.GetCellUpCenterPosition(x, y), World.UpQuaternion());
                }
                else if ((y < Settings.MapWidth - 1) && (Global.map[x, y].mapRoom != Global.map[x, y + 1].mapRoom))
                {
                    Instantiate(wallUp, World.GetCellUpCenterPosition(x, y), World.UpQuaternion());
                }
                    
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
