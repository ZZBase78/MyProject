using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Global
{
    public static MapPoint[,] map;

    public static List<MapRoom> mapRooms;

    public static GameObject[] prefabs;

    public static MapRoom startRoom;

    public static MapRoom endRoom;

    public static List<MapDoor> mapDoors;

    public static List<Enemy> enemies;

    public static Texture[] textures;

    public static bool IsInterval(int value, int min, int max)
    {
        return ((value >= min) && (value <= max));
    }
}
