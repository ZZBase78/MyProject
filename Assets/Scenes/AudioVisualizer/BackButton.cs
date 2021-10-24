using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) SceneManager.LoadScene(0);
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 150, 30), "Главное меню")) SceneManager.LoadScene(0);
    }
}
