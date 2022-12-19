using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnReSpawn : MonoBehaviour
{
    public GameObject Enemy;
    public Transform EnemyPosition;
    public int SpawnCountMax;       // Spawner¿¡¼­ ¼ÒÈ¯ ÃÖ´ë °¹¼ö
    public float initialSpawnDelay;  // ÃÊ±â ¼ÒÈ¯ µô·¹ÀÌ
    public float spawnDelay;     // ¹Ýº¹ ¼ÒÈ¯ µô·¹ÀÌ
    public int enemyMaxCount;   // ¾À¿¡ Á¸ÀçÇÒ ÀûÀÇ ÃÑ °¹¼ö
    GameObject[] enemies;
    int SpawnCount;
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
            enemies= GameObject.FindGameObjectsWithTag("Enemy");
            if (enemies.Length < enemyMaxCount)
            {
                StartCoroutine(CreateNewObject());
                StopCoroutine(CreateNewObject());
            }
            var--;
            StartCoroutine(Delay());
        }
    }
    IEnumerator CreateNewObject()
    {
        if (SpawnCount < SpawnCountMax)
        {
            GameObject PoolEnemy = Instantiate(Enemy, EnemyPosition.position, transform.rotation);
            SpawnCount++;            
            yield return new WaitForSeconds(spawnDelay);
        }

    }
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(spawnDelay + 1f); ;
        var++;

    }
}
