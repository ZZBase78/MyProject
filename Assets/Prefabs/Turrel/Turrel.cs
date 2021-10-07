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
        status = 1; // ����� ����������
    }

    // Update is called once per frame
    void Update()
    {
        if (status == 1)
        {
            //����� ����������, �������� �� ������� ���������� ���������� � ���� ���������
            if (turrelFindTargetControl.target_found)
            {
                //��������� ������
                target = turrelFindTargetControl.target_point;
                target_found = turrelFindTargetControl.target_found;
                status = 2; // ������ ��������� ������� � ���������� ����������
            }
        }
        else if (status == 2)
        {
            //�������� ������ � ���������� ����������
            turrelLampControl.SetFlashing(2, 0, 0.1f, 0.1f, 1f, 2);
            turrelRotateControl.ChangeStatus(0); //������������� ��������
            status = 3; // �������� ��������� ��������� �������
        }
        else if (status == 3)
        {
            //�������� ��������� ��������� �������, ���� ���� ��������
            if (!turrelFindTargetControl.target_found)
            {
                target = Vector3.zero;
                target_found = false;
                turrelLampControl.SetColor(1, true);
                turrelRotateControl.ChangeStatus(1);
                status = 1; //����� ����������
            }
            else if (turrelLampControl.status != 2)
            {
                //��������� ������
                status = 4; // ����� ��������� ��� ��������
            }
        }
        else if (status == 4)
        {
            // ����� ��������� ��� ��������
            turrelRotateControl.SetNavigateNoFire(target);
            status = 5; // �������� ��������� 
        }
        else if (status == 5)
        {
            //�������� ��������� ���� ���� ��������
            if (!turrelFindTargetControl.target_found)
            {
                target = Vector3.zero;
                target_found = false;
                turrelLampControl.SetColor(1, true);
                turrelRotateControl.ChangeStatus(1);
                status = 1; //����� ����������
            }
            else if (turrelRotateControl.status == 3) // ������ ��������
            {
                turrelLampControl.SetColor(3, true);
                World.PlayClip(audios[0], clips[0]);
                status = 6; // ��������
            }
        }
        else if (status == 6)
        {
            //�������� + ���������, ���� ���� ��������
            if (!turrelFindTargetControl.target_found)
            {
                target = Vector3.zero;
                target_found = false;
                turrelLampControl.SetColor(1, true);
                turrelRotateControl.ChangeStatus(1);
                status = 1; //����� ����������
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
