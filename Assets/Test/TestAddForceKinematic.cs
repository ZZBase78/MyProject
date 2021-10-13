using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAddForceKinematic : MonoBehaviour
{

    Rigidbody _rb;

    public float force;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            _rb.AddForce(transform.up * force);
        }
    }
}
