using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Canvas_GameOver : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        Global.Cursor_On();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Button_Click()
    {
        SceneManager.LoadScene(0);
    }

}
