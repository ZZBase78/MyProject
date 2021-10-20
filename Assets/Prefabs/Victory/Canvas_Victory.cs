using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Canvas_Victory : MonoBehaviour
{

    public void Button_Click()
    {
        SceneManager.LoadScene(0);
    }

    // Start is called before the first frame update
    void Start()
    {
        Global.Cursor_On();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
