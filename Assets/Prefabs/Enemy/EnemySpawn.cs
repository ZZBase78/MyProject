using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{

    public List<GameObject> enemy_prefabs;

    public int MaxEnemy = 10;

    private void Awake()
    {
        Global.enemies = new List<Enemy>();
    }

    void SpawnEnemy()
    {
        int x = Random.Range(0, Settings.MapWidth);
        int y = Random.Range(0, Settings.MapHeight);
        Vector3 position = World.GetCellPosition(x, y);

        int prefab_index = Random.Range(0, enemy_prefabs.Count);
        GameObject enemy_prefab = enemy_prefabs[prefab_index];

        GameObject go = Instantiate(enemy_prefab, position, Quaternion.Euler(0, Random.Range(0, 360), 0));

        Enemy enemy = go.GetComponent<Enemy>();

        Global.enemies.Add(enemy);

    }

    private void FixedUpdate()
    {
        if (Global.enemies.Count < MaxEnemy) SpawnEnemy();
    }
}
