using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurrelRotateControl : MonoBehaviour
{

    public AudioSource[] audios;

    public AudioClip[] clips;

    public int status; // 0 - ничего; 1 - поиск //2 навигация к цели //3 - достигнута навигация к цели
    public GameObject rotationBase;
    public GameObject rotationGun;

    Vector3 finding_target;
    int finding_status; //0 - пауза //1 - движение к цели
    float starttimefinding0 = 1;
    float currenttimefinding0;
    public void ChangeStatus(int new_status)
    {
        status = new_status;

        if (status == 1)
        {
            finding_status = 0;
            currenttimefinding0 = starttimefinding0;
        }else if (status == 0)
        {
            World.PlayClip(audios[0], clips[1]);
        }
    }

    public void SetNavigateNoFire(Vector3 new_target)
    {
        status = 2;
        finding_target = new_target;
    }

    void Finding()
    {
        if (finding_status == 0)
        {
            currenttimefinding0 -= Time.deltaTime;
            if (currenttimefinding0 <= 0)
            {
                finding_target = rotationGun.transform.position + (new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)));
                finding_status = 1;
            }
        } else if (finding_status == 1)
        {

            if (audios[0].clip != clips[0] || !audios[0].isPlaying) World.PlayClip(audios[0], clips[0]);

            Vector3 direction = finding_target - rotationGun.transform.position;
            Vector3 directionBase = direction; directionBase.y = 0;
            Vector3 directionGun = Vector3.up * (direction.y / directionBase.magnitude);

            rotationBase.transform.rotation = Quaternion.RotateTowards(rotationBase.transform.rotation, Quaternion.LookRotation(directionBase), 50 * Time.deltaTime);
            rotationGun.transform.rotation = Quaternion.RotateTowards(rotationGun.transform.rotation, Quaternion.LookRotation(rotationBase.transform.forward + directionGun), 50 * Time.deltaTime);
            if (Vector3.Angle(rotationGun.transform.forward, direction) < 0.5f){
                World.PlayClip(audios[0], clips[1]);
                finding_status = 0;
                currenttimefinding0 = starttimefinding0;
            }
        }
    }

    void NavigateNoFire()
    {
        if (audios[0].clip != clips[0] || !audios[0].isPlaying) World.PlayClip(audios[0], clips[0]);

        Vector3 direction = finding_target - rotationGun.transform.position;
        Vector3 directionBase = direction; directionBase.y = 0;
        Vector3 directionGun = Vector3.up * (direction.y / directionBase.magnitude);

        rotationBase.transform.rotation = Quaternion.RotateTowards(rotationBase.transform.rotation, Quaternion.LookRotation(directionBase), 50 * Time.deltaTime);
        rotationGun.transform.rotation = Quaternion.RotateTowards(rotationGun.transform.rotation, Quaternion.LookRotation(rotationBase.transform.forward + directionGun), 50 * Time.deltaTime);
        if (Vector3.Angle(rotationGun.transform.forward, direction) < 0.5f)
        {
            World.PlayClip(audios[0], clips[1]);
            status = 3;
        }
    }

    void TestControl()
    {
        if (Input.GetKey(KeyCode.Alpha0)) ChangeStatus(1);
    }

    private void Start()
    {
        //ChangeStatus(1);
    }
    private void Update()
    {

        //TestControl();

        if (status == 1)
        {
            Finding();
        }else if (status == 2)
        {
            NavigateNoFire();
        }
    }

}
