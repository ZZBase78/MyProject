using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Global
{
    public static bool game_paused;

    public static GameObject drob_spawner;

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

    public static int Enemy2_killed;

    public static Enemy2Spawner enemy2Spawner;

    public static GameObject player;
    public static PlayerMove player_script;

    public static List<GameObject> lamps;

    public static Camera_3d camera_3d;

    public static GameObject pause_menu;

    public static int camera_pixel_width;
    public static int camera_pixel_height;
    public static float mouse_speed;

    public static GameObject canvas_console;


    public static void SetCameraPixelValues()
    {
        camera_pixel_width = Camera.main.pixelWidth;
        camera_pixel_height = Camera.main.pixelHeight;
    }

    public static void SetPauseGame(bool value)
    {
        if (value == true)
        {
            Cursor_On();
            Global.game_paused = true;
            pause_menu = GameObject.Instantiate(Global.prefabs[16]);
            canvas_console.SetActive(false);
            Time.timeScale = 0;
        }
        else
        {
            Cursor_Off();
            Global.game_paused = false;
            GameObject.Destroy(pause_menu);
            canvas_console.SetActive(true);
            Time.timeScale = 1;
        }
    }

    public static void Cursor_On()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public static void Cursor_Off()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public static void SelAllLampOn()
    {
        if (lamps == null) return;
        foreach (GameObject lamp_go in lamps)
        {
            if (lamp_go == null) continue;
            Lamp lamp_script = lamp_go.GetComponent<Lamp>();
            if (lamp_script != null)
            {
                lamp_script.mode = 0;
                lamp_script.go_light.GetComponent<Light>().intensity = 1;
                lamp_script.go_light.SetActive(true);
                lamp_script.go_spotlight.SetActive(true);
            }
        }
    }

    public static bool IsInterval(int value, int min, int max)
    {
        return ((value >= min) && (value <= max));
    }
    public static bool IsInterval(float value, float min, float max)
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
