using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Canvas_Console : MonoBehaviour
{

    public float max_health;
    public float current_health;

    public Image panel_health;

    private void Awake()
    {
        Global.canvas_console = gameObject;
        max_health = Settings.player_max_health;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        panel_health.fillAmount = (float)Global.player_script.health / max_health;
        panel_health.color = new Color(1f, panel_health.fillAmount, panel_health.fillAmount);
    }
}
