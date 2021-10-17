using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drob_spawner : MonoBehaviour
{

    GameObject drob;

    private void Awake()
    {
        Global.drob_spawner = gameObject;
    }

    Vector3 GetRandomPosition()
    {
        float x = Random.Range(0f, 27f);
        float z = Random.Range(-7f, -3f);
        return new Vector3(x, 0.5f, z);
    }

    void GenerateDrob()
    {
        Vector3 position = GetRandomPosition();
        drob = new GameObject();
        drob.transform.position = position;
        Instantiate(Global.prefabs[14], position, Quaternion.identity, drob.transform);
    }

    // Start is called before the first frame update
    void Start()
    {
        GenerateDrob();
    }

    // Update is called once per frame
    void Update()
    {
        if (drob == null && !IsInvoking("GenerateDrob"))
        {
            Invoke("GenerateDrob", 60f);
        }
    }

    public void Drob_PickedUp(GameObject picked_drob)
    {
        Destroy(picked_drob);
        drob = null;
        Global.player.GetComponent<PlayerMove>().Drob_pickup();
    }
}
