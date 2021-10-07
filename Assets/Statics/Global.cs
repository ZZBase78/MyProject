using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Global
{

    public static List<MapTurrel> mapTurrels;

    public static MapPoint[,] map;

    public static List<MapRoom> mapRooms;

    public static GameObject[] prefabs;

    public static MapRoom startRoom;

    public static MapRoom endRoom;

    public static List<MapDoor> mapDoors;

    public static List<Enemy> enemies;

    public static Texture[] textures;

    public static List<MapKey> mapKeys;
    public static List<MapKey> collectKeys;
    public static List<MapKey> usedKeys;

    public static AudioClip[] clips;

    public static bool IsInterval(int value, int min, int max)
    {
        return ((value >= min) && (value <= max));
    }

    public static bool KeyPreset(int x, int y)
    {
        foreach (MapKey mapKey in mapKeys)
        {
            if (mapKey.x == x && mapKey.y == y) return true;
        }
        return false;
    }

    public static bool TurrelPreset(int x, int y)
    {
        foreach (MapTurrel mapturrel in mapTurrels)
        {
            if (mapturrel.x == x && mapturrel.y == y) return true;
        }
        return false;
    }

    public static MapKey GetCollectKey(int key_id)
    {
        foreach (MapKey mapKey in collectKeys)
        {
            if (mapKey.key_id == key_id) return mapKey;
        }
        return null;
    }
}
