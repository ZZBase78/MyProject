using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPoint
{

    public MapRoom mapRoom;
    public int x;
    public int y;

    public MapDoor door_up;
    public MapDoor door_down;
    public MapDoor door_left;
    public MapDoor door_right;

    public MapPoint(int x, int y)
    {
        this.x = x;
        this.y = y;
        mapRoom = null;
    }
}
