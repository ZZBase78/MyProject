using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator10k : MonoBehaviour
{

    List<GameObject> list;

    // Start is called before the first frame update
    void Start()
    {

        list = new List<GameObject>();

        for (int i = 0; i < 10000; i++)
        {
            GameObject go = new GameObject();
            go.transform.parent = transform;
            list.Add(go);
        }

    }

    // Update is called once per frame
    void Update()
    {
        foreach(GameObject go in list)
        {
            go.transform.Rotate(0f, 90f * Time.deltaTime, 0f);
        }
    }
}
