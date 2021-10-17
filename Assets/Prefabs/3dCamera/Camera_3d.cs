using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_3d : MonoBehaviour
{

    GameObject player;

    Camera _camera;
    AudioListener _audioListener;

    Vector3 target_position;


    private void Awake()
    {
        Global.camera_3d = this;
        _camera = GetComponent<Camera>();
        _audioListener = GetComponent<AudioListener>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void TurnOn()
    {
        _camera.enabled = true;
        _audioListener.enabled = true;
    }

    public void TurnOff()
    {
        _camera.enabled = false;
        _audioListener.enabled = false;
    }

    void GetPlayer()
    {
        if (player != null) return;

        if (Global.player != null) player = Global.player;
    }

    void LookAtPlayer()
    {
        if (player == null) return;

        transform.LookAt(player.transform.position + Vector3.up * 1.5f);    
    }

    bool CheckDirection(Vector3 from, Vector3 direction, float distance)
    {
        if (Physics.Raycast(from, direction, out RaycastHit hit, distance))
        {
            if (hit.distance >= 2f)
            {
                target_position = hit.point;
                return true;
            }
        }
        else
        {
            target_position = from + direction.normalized * distance;
            return true;
        }
        return false;
    }

    void FindPosition()
    {
        if (player == null) return;

        Vector3 look_at_position = player.transform.position + Vector3.up * 1.5f;
        Vector3 player_forward = player.transform.forward;
        player_forward.x = 0;
        player_forward.z = 0;
        player_forward = player_forward.normalized;

        Vector3 direction;

        direction = (-player_forward + Vector3.up);
        if (CheckDirection(look_at_position, direction, 10f)) return;

        direction = (-2f * player_forward + Vector3.up);
        if (CheckDirection(look_at_position, direction, 10f)) return;

        direction = (-player_forward + 2f * Vector3.up);
        if (CheckDirection(look_at_position, direction, 10f)) return;

        direction = (-player_forward);
        if (CheckDirection(look_at_position, direction, 10f)) return;

        direction = (player.transform.right + Vector3.up);
        if (CheckDirection(look_at_position, direction, 10f)) return;

        direction = (player.transform.right);
        if (CheckDirection(look_at_position, direction, 10f)) return;

        direction = (player_forward + Vector3.up);
        if (CheckDirection(look_at_position, direction, 10f)) return;

        direction = (player_forward);
        if (CheckDirection(look_at_position, direction, 10f)) return;

        //в крайних случаях
        target_position = look_at_position;
    }

    void MoveCamera()
    {
        transform.position = Vector3.Lerp(transform.position, target_position, 5f * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        GetPlayer();

        LookAtPlayer();

        FindPosition();

        MoveCamera();
    }
}
