using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject Enemy;
    public Transform EnemyPosition;
    public int SpawnCountMax;
    int SpawnCount;
    public float initialSpawnDelay;
    public float spawnDelay;
    int var;

   
    private void Awake()
    {
        SpawnCount = 0;
        var = 1;
    }

    void Start()
    {
               
    }
    private void Update()
    {
        if (initialSpawnDelay >= 0)
        {
            initialSpawnDelay -= Time.deltaTime;
        }

        if (initialSpawnDelay <= 0 && var == 1)
        {
            StartCoroutine(CreateNewObject());
            StopCoroutine(CreateNewObject());
            var--;
        }
    }
    IEnumerator CreateNewObject()
    {
        while (SpawnCount < SpawnCountMax)
        {
            GameObject PoolEnemy = Instantiate(Enemy, EnemyPosition.position, transform.rotation);
            SpawnCount++;
            yield return new WaitForSeconds(spawnDelay);
        }

    }
}
