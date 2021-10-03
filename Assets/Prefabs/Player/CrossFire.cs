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
            door.ChangeOpen(true);
            door.map_door.coonecting_door.door_up.ChangeOpen(false);
        }
    }
    void OnGUI()
    {
        GUI.Label(r, drawing_texture);
    }
}
