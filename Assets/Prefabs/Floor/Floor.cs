using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{

    public GameObject floor;

    public int floor_txture_id;

    // Start is called before the first frame update
    void Start()
    {
        World.SetTextureXZ(floor, floor_txture_id);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
