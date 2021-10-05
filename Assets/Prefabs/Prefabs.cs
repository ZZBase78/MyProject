using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prefabs : MyRoot
{

    public GameObject[] prefabs;

    public Texture[] textures;

    public AudioClip[] clips;

    void Awake()
    {
        Global.prefabs = prefabs;
        Global.textures = textures;
        Global.clips = clips;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
