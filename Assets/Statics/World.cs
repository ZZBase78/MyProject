using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class World
{
    public static void PlayClip(Transform transform, int clip_index)
    {
        GameObject go = GameObject.Instantiate(Global.prefabs[11], transform.position, Quaternion.identity, transform);//audio source
        AudioSource _audio = go.GetComponent<AudioSource>();
        _audio.clip = Global.clips[clip_index];
        _audio.Play();
        GameObject.Destroy(go, _audio.clip.length * 2);
    }

    public static MapPoint GetMapPosition(Vector3 position)
    {
        int x = (int)Mathf.Round(position.x / Settings.CellWidth);
        int y = (int)Mathf.Round(position.z / Settings.CellWidth);

        return Global.map[x, y];
    }

    public static float GetCellXPosition(int x)
    {
        return Settings.CellWidth * x;
    }
    public static float GetCellYPosition(int y)
    {
        return Settings.CellHeight * y;
    }
    public static Vector3 GetCellPosition(int x, int y)
    {
        return new Vector3(GetCellXPosition(x), 0, GetCellYPosition(y));
    }
    public static Vector3 GetCellLeftCenterPosition(int x, int y)
    {
        return new Vector3(GetCellXPosition(x) - (Settings.CellWidth / 2), 0, GetCellYPosition(y));
    }
    public static Vector3 GetCellRightCenterPosition(int x, int y)
    {
        return new Vector3(GetCellXPosition(x) + (Settings.CellWidth / 2), 0, GetCellYPosition(y));
    }
    public static Vector3 GetCellUpCenterPosition(int x, int y)
    {
        return new Vector3(GetCellXPosition(x), 0, GetCellYPosition(y) + (Settings.CellHeight / 2));
    }
    public static Vector3 GetCellDownCenterPosition(int x, int y)
    {
        return new Vector3(GetCellXPosition(x), 0, GetCellYPosition(y) - (Settings.CellHeight / 2));
    }
    public static Vector3 GetCellLeftUpPosition(int x, int y)
    {
        return new Vector3(GetCellXPosition(x) - (Settings.CellWidth / 2), 0, GetCellYPosition(y) + (Settings.CellHeight / 2));
    }
    public static Vector3 GetCellRightUpPosition(int x, int y)
    {
        return new Vector3(GetCellXPosition(x) + (Settings.CellWidth / 2), 0, GetCellYPosition(y) + (Settings.CellHeight / 2));
    }
    public static Vector3 GetCellLeftDownPosition(int x, int y)
    {
        return new Vector3(GetCellXPosition(x) - (Settings.CellWidth / 2), 0, GetCellYPosition(y) - (Settings.CellHeight / 2));
    }
    public static Vector3 GetCellRightDownPosition(int x, int y)
    {
        return new Vector3(GetCellXPosition(x) + (Settings.CellWidth / 2), 0, GetCellYPosition(y) - (Settings.CellHeight / 2));
    }
    public static Quaternion UpQuaternion()
    {
        return Quaternion.identity;
    }
    public static Quaternion DownQuaternion()
    {
        return Quaternion.Euler(0, 180, 0);
    }
    public static Quaternion LeftQuaternion()
    {
        return Quaternion.Euler(0, -90, 0);
    }
    public static Quaternion RightQuaternion()
    {
        return Quaternion.Euler(0, 90, 0);
    }

    public static void SetTextureXY(GameObject go, int texture_id)
    {
        Material m = go.GetComponent<Renderer>().material;
        m.mainTexture = Global.textures[texture_id];
        m.mainTextureScale = new Vector2(go.transform.localScale.x, go.transform.localScale.y);
    }
    public static void SetTextureXZ(GameObject go, int texture_id)
    {
        Material m = go.GetComponent<Renderer>().material;
        m.mainTexture = Global.textures[texture_id];
        m.mainTextureScale = new Vector2(go.transform.localScale.x, go.transform.localScale.z);
    }
}
