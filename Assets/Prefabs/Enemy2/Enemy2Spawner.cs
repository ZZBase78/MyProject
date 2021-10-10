using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2Spawner : MonoBehaviour
{

    bool on;

    int enemy_count;
    int max_enemy;

    public GameObject enemy2_prefab;

    public GameObject[] spawner_points;
    public int current_index_spawn;

    public void InstantiateEnemy()
    {
        current_index_spawn = (current_index_spawn + 1) % spawner_points.Length;
        GameObject.Instantiate(enemy2_prefab, spawner_points[current_index_spawn].transform.position, Quaternion.identity);
        enemy_count++;
    }

    public void Destroy_Enemy(GameObject enemy_gameobject)
    {
        GameObject.Destroy(enemy_gameobject);
        enemy_count--;
    }

    private void Awake()
    {
        Global.enemy2Spawner = this;
        current_index_spawn = 0;
        max_enemy = 10;
        on = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (on && enemy_count < max_enemy) InstantiateEnemy();
        if (Global.player != null && Global.player.transform.position.z > -6) // проверка что игрок дошел до входной двери
        {
            on = false;
        }
    }
}
