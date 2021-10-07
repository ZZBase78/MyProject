using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTurrel
{
    public int x;
    public int y;

    public GameObject go;

    public MapTurrel()
    {

    }

    public bool GenerateCoordinates()
    {
        for (int i = 0; i < 100; i++) // 100 попыток разместить турель
        {
            int _x = Random.Range(0, Settings.MapWidth);
            int _y = Random.Range(0, Settings.MapHeight);
            if (Global.KeyPreset(_x, _y)) continue;
            if (Global.TurrelPreset(_x, _y)) continue;
            x = _x;
            y = _y;
            return true;
        }

        return false;
    }

    public void Instantiate()
    {
        go = GameObject.Instantiate(Global.prefabs[12], World.GetCellPosition(x, y), Quaternion.identity);
        Turrel turrel = go.GetComponent<Turrel>();
        turrel.mapTurrel = this;
    }
}
