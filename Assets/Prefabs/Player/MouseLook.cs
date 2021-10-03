using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{

    public bool enable_X;

    public bool enable_Y;

    public bool enable_limit_y;

    public float min_y_euler;
    public float max_y_euler;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (enable_X)
        {
            float rotation_x = Input.GetAxis("Mouse X") * 20;
            transform.Rotate(0, rotation_x, 0);
        }

        if (enable_Y)
        {
            float rotation_y = -Input.GetAxis("Mouse Y") * 20;

            if (enable_limit_y)
            {
                float eulerx = transform.localEulerAngles.x + rotation_y;

                if (eulerx > -min_y_euler && eulerx <= 180)
                {
                    eulerx = -min_y_euler;
                }
                if (eulerx > 180 && eulerx < 360 - max_y_euler)
                {
                    eulerx = 360 - max_y_euler;
                }

                transform.localEulerAngles = new Vector3(eulerx, transform.rotation.y, transform.rotation.z);
            }
            else
            {
                transform.Rotate(rotation_y, 0, 0);
            }

            
        }

    }
}
