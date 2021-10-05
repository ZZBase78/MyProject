using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishPrize : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            World.PlayClip(transform, 3);
        }
    }
}
