using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{

    public MapKey mapKey;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Global.collectKeys.Add(mapKey);
            GameObject go = Instantiate(Global.prefabs[11], other.gameObject.transform.position, Quaternion.identity, other.gameObject.transform);
            AudioSource _audio = go.GetComponent<AudioSource>();
            _audio.clip = Global.clips[0];
            _audio.Play();
            Destroy(go, 2);
            Destroy(gameObject);
        }
    }
}
