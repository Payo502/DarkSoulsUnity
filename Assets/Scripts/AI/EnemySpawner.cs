using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;

    [SerializeField] private float enemyInterval = 3.5f;
    public float maxAmountOfEnemies = 1;
    private float enemyCount;

    private void Start()
    {

        StartCoroutine(SpawnEnemy(enemyInterval, enemyPrefab));


    }

    private IEnumerator SpawnEnemy(float interval, GameObject enemy)
    {
        if (enemyCount < maxAmountOfEnemies)
        {
            yield return new WaitForSeconds(interval);
            GameObject newEnemy = Instantiate(enemy, transform.position, Quaternion.identity);
            enemyCount++;
            StartCoroutine(SpawnEnemy(interval, enemy));
        }

    }
}
