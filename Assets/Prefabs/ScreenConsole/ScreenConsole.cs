using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenConsole : MonoBehaviour
{

    private void OnGUI()
    {

        float y = Screen.height - (Global.collectKeys.Count * 20) - 20;

        foreach (MapKey mapKey in Global.collectKeys)
        {
            if (Global.usedKeys.Contains(mapKey))
            {
                //GUI.Label(new Rect(10, y, 200, y + 20), mapKey.ToString() + " (использован)");
                GUI.Label(new Rect(10, y, 200, 20), mapKey.ToString() + " (использован)");
            }
            else
            {
                //GUI.Label(new Rect(10, y, 200, y + 20), mapKey.ToString());
                GUI.Label(new Rect(10, y, 200, 20), mapKey.ToString());
            }
            
            y += 20;
        }

        GUI.Label(new Rect(Screen.width - 200, Screen.height - 40, 200, 20), "¬рагов в лабиринте: " + Global.enemies.Count);
        GUI.Label(new Rect(Screen.width - 200, Screen.height - 20, 200, 20), "”бито на открытом уровне: " + Global.Enemy2_killed);

        //if (Global.player != null)
        //{
        //    GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height - 20, 200, 20), "«ƒќ–ќ¬№≈: " + Global.player_script.health);
        //}


        
    }
}
