using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roof : MonoBehaviour
{
    public GameObject roof;

    public int roof_txture_id;

    // Start is called before the first frame update
    void Start()
    {
        World.SetTextureXZ(roof, roof_txture_id);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
