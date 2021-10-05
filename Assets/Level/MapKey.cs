using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapKey
{
    public int key_id;

    public int x;
    public int y;

    public GameObject gameObject;

    public MapKey()
    {

    }

    public void Generate(int new_key_id)
    {
        do
        {
            int _x = Random.Range(0, Settings.MapWidth);
            int _y = Random.Range(0, Settings.MapHeight);
            if ((Global.map[_x, _y].mapRoom.room_id != 0) && (Global.map[_x, _y].mapRoom.room_id <= new_key_id) && !Global.KeyPreset(_x, _y))
            {
                key_id = new_key_id;
                x = _x;
                y = _y;
                break;
            }
        } while (true); //не должно зациклится
    }

    public void Instantiate()
    {
        gameObject = GameObject.Instantiate(Global.prefabs[10], World.GetCellPosition(x, y), Quaternion.identity);
        Key key = gameObject.GetComponent<Key>();
        key.mapKey = this;
    }

    public override string ToString()
    {
        return $"Ключ №{key_id}";
    }
}
