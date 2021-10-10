using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurrelFindTargetControl : MonoBehaviour
{

    public GameObject bulletSpawn;

    //GameObject target_object;

    public Vector3 target_point;
    public bool target_found;

    private void Awake()
    {
        //list_targets = new List<GameObject>();
    }

    private void Update()
    {
        //if (target_object != null)
        //{
        //    target_point = target_object.transform.position;
        //}

        if (Global.player != null)
        {
            // игрок на сцене

            Vector3 direction = (Global.player.transform.position + Vector3.up - bulletSpawn.transform.position);
            if (direction.magnitude <= 10f)
            {
                // до игрока менее мене 10м
                RaycastHit[] raycasts = Physics.RaycastAll(bulletSpawn.transform.position, direction, direction.magnitude);
                bool find_obstacle = false;
                foreach(RaycastHit hitinfo in raycasts)
                {
                    if (hitinfo.transform.tag == "Turrel") continue;
                    if (hitinfo.transform.gameObject == Global.player) continue;
                    find_obstacle = true;
                    //Debug.Log("Turrel obstacle: " + hitinfo.transform.name);
                    break;
                }
                if (find_obstacle)
                {
                    target_found = false;
                    target_point = Vector3.zero;
                }
                else
                {
                    target_found = true;
                    target_point = Global.player.transform.position + Vector3.up;
                }
            }
            else
            {
                target_found = false;
                target_point = Vector3.zero;
            }
        }
        else
        {
            target_found = false;
            target_point = Vector3.zero;
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        //list_targets.Add(other.gameObject.transform.root.gameObject);

        //if (other.transform.root.CompareTag("Player")) {
        //    target_object = other.transform.root.gameObject;
        //}
    }

    private void OnTriggerExit(Collider other)
    {
        //if (other.transform.root.CompareTag("Player"))
        //{
        //    target_found = false;
        //    target_point = Vector3.zero;
        //    target_object = null;
        //}
    }

    private void OnTriggerStay(Collider other)
    {
        //if (target_object != null)
        //{
        //    Vector3 direction = target_object.transform.position - bulletSpawn.transform.position;
        //    if (Physics.Raycast(bulletSpawn.transform.position, direction, out RaycastHit hitInfo, direction.magnitude))
        //    {
        //        if (hitInfo.transform.name == "BigGun" || hitInfo.transform.name == "SmallGun" || hitInfo.transform.name == "TurrelBase") return;
        //        if (hitInfo.transform.root.CompareTag("Player"))
        //        {

        //            target_found = true;
        //            target_point = hitInfo.point;
        //        }
        //        else
        //        {
        //            target_found = false;
        //            target_point = Vector3.zero;
        //        }
        //    }
        //}
    }
}
