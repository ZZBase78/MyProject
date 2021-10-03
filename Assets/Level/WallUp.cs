using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallUp : MonoBehaviour
{

    public GameObject center;
    public GameObject left;
    public GameObject right;

    public int texture_id;

    // Start is called before the first frame update
    void Start()
    {
        World.SetTextureXY(center, texture_id);
        World.SetTextureXY(left, texture_id);
        World.SetTextureXY(right, texture_id);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
