using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineDamageDetector : MonoBehaviour
{

    public List<IDamagable> list;

    private void Awake()
    {
        list = new List<IDamagable>();
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamagable i = other.gameObject.GetComponentInParent<IDamagable>();
        if (i != null && !list.Contains(i))
        {
            list.Add(i);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IDamagable i = other.gameObject.GetComponentInParent<IDamagable>();
        if (i != null && list.Contains(i))
        {
            list.Remove(i);
        }
    }
}
