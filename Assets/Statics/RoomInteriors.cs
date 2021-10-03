using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RoomInteriors
{
    public static RoomInterior GetRandom()
    {
        RoomInterior interior = new RoomInterior();
        interior.wall = Random.Range(0, Global.textures.Length);
        do
        {
            interior.door = Random.Range(0, Global.textures.Length);
        } while (interior.door == interior.wall);
        
        interior.floor = Random.Range(0, Global.textures.Length);
        interior.roof = Random.Range(0, Global.textures.Length);
        return interior;
    }
}
