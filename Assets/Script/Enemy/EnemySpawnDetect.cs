using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemySpawnDetect : MonoBehaviour
{
    public GameObject Enemy;
    public Transform EnemyPosition;
    public int SpawnCountMax;
    int SpawnCount;
    public float initialSpawnDelay;
    public float spawnDelay;
    int var;
    public Transform DetectBox;
    public Vector2 boxSize;

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

        Collider2D[] collider2D = Physics2D.OverlapBoxAll(DetectBox.position, boxSize, 0);
        foreach (Collider2D collider in collider2D)
        {
            if (collider.CompareTag("Player"))
            {
                if (initialSpawnDelay <= 0 && var == 1)
                {
                    StartCoroutine(CreateNewObject());
                    StopCoroutine(CreateNewObject());
                    var--;
                }
            }
        }
                if (initialSpawnDelay >= 0)
        {
            initialSpawnDelay -= Time.deltaTime;
        }

    }
    IEnumerator CreateNewObject()
    {
        if (SpawnCount < SpawnCountMax)
        {
            yield return new WaitForSeconds(spawnDelay);
            GameObject PoolEnemy = Instantiate(Enemy, EnemyPosition.position, transform.rotation);
            PoolEnemy.SetActive(true);
            SpawnCount++;
            var = 1;
        }

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(DetectBox.position, boxSize);
    }
}
