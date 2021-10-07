using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turrel : MonoBehaviour
{

    public MapTurrel mapTurrel;

    public AudioSource[] audios;
    public AudioClip[] clips;

    public TurrelLampControl turrelLampControl;
    public TurrelRotateControl turrelRotateControl;
    public TurrelFindTargetControl turrelFindTargetControl;
    public TurrelFireControl turrelFireControl;

    public int status;

    public Vector3 target;
    public bool target_found;

    // Start is called before the first frame update
    void Start()
    {
        turrelLampControl.SetColor(1, true);
        turrelRotateControl.ChangeStatus(1);
        status = 1; // Поиск противника
    }

    // Update is called once per frame
    void Update()
    {
        if (status == 1)
        {
            //Поиск противника, делается до момента нахождения противника в зоне видимости
            if (turrelFindTargetControl.target_found)
            {
                //противник найден
                target = turrelFindTargetControl.target_point;
                target_found = turrelFindTargetControl.target_found;
                status = 2; // Подача звукового сигнала о нахождении противника
            }
        }
        else if (status == 2)
        {
            //Звуковой сигнал о нахождении противника
            turrelLampControl.SetFlashing(2, 0, 0.1f, 0.1f, 1f, 2);
            turrelRotateControl.ChangeStatus(0); //останавливаем движение
            status = 3; // ожидание окончания звукового сигнала
        }
        else if (status == 3)
        {
            //ожидание окончания звукового сигнала, либо цель потеряна
            if (!turrelFindTargetControl.target_found)
            {
                target = Vector3.zero;
                target_found = false;
                turrelLampControl.SetColor(1, true);
                turrelRotateControl.ChangeStatus(1);
                status = 1; //поиск противника
            }
            else if (turrelLampControl.status != 2)
            {
                //Закончили мигать
                status = 4; // режим наведения без стрельбы
            }
        }
        else if (status == 4)
        {
            // режим наведения без стрельбы
            turrelRotateControl.SetNavigateNoFire(target);
            status = 5; // ожидание наведения 
        }
        else if (status == 5)
        {
            //ожидание наведения либо цель потеряня
            if (!turrelFindTargetControl.target_found)
            {
                target = Vector3.zero;
                target_found = false;
                turrelLampControl.SetColor(1, true);
                turrelRotateControl.ChangeStatus(1);
                status = 1; //поиск противника
            }
            else if (turrelRotateControl.status == 3) // орудие навелось
            {
                turrelLampControl.SetColor(3, true);
                World.PlayClip(audios[0], clips[0]);
                status = 6; // стрельба
            }
        }
        else if (status == 6)
        {
            //стрельба + наведение, либо цель потеряна
            if (!turrelFindTargetControl.target_found)
            {
                target = Vector3.zero;
                target_found = false;
                turrelLampControl.SetColor(1, true);
                turrelRotateControl.ChangeStatus(1);
                status = 1; //поиск противника
            }
            else
            {
                target = turrelFindTargetControl.target_point;
                turrelRotateControl.SetNavigateNoFire(target);
                turrelFireControl.Fire(target);
            }
        }
    }
}
