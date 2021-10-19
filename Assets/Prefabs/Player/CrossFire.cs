using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossFire : MonoBehaviour
{
    public Texture2D crosshairTexture;
    public Texture2D crosshairTextureE;

    private Rect r;

    Texture2D drawing_texture;

    void Update()
    {
        if (Global.game_paused) return;

        drawing_texture = crosshairTexture;

        DoorUp door = null;

        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, 2f))
        {
            if (hitInfo.transform.tag.Equals("Door"))
            {
                door = hitInfo.transform.GetComponentInParent<DoorUp>();
                if (door != null) drawing_texture = crosshairTextureE;
            }
        }

        r = new Rect((Screen.width / 2) - (crosshairTexture.width / 2), (Screen.height / 2) - (crosshairTexture.height / 2), crosshairTexture.width, crosshairTexture.height);

        if (Input.GetKeyDown(KeyCode.E) && door != null)
        {
            if (door.map_door.open)
            {
                door.ChangeOpen(true);
                door.map_door.coonecting_door.door_up.ChangeOpen(false);
            }
            else
            {
                MapKey mapKey = Global.GetCollectKey(door.map_door.door_id);
                if (mapKey == null)
                {
                    GameObject go = Instantiate(Global.prefabs[11], transform.position, Quaternion.identity, transform);
                    AudioSource _audio = go.GetComponent<AudioSource>();
                    _audio.clip = Global.clips[1];
                    _audio.Play();
                    Destroy(go, 2);
                }
                else
                {
                    GameObject go = Instantiate(Global.prefabs[11], transform.position, Quaternion.identity, transform);
                    AudioSource _audio = go.GetComponent<AudioSource>();
                    _audio.clip = Global.clips[2];
                    _audio.Play();
                    Destroy(go, 2);

                    Global.usedKeys.Add(mapKey);
                    door.map_door.SetOpen(true);

                    door.ChangeOpen(true);
                    door.map_door.coonecting_door.door_up.ChangeOpen(false);
                }
            }
        }
    }
    void OnGUI()
    {
        if (Global.game_paused) return;

        GUI.Label(r, drawing_texture);
    }
}
