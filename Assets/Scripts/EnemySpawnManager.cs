using System.Collections;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    private Transform enemyContainer;
    private Coroutine spawnCoroutine;
    private Coroutine spawnRateIncreaseCoroutine;
    private float currentSecondsBetweenSpawn;
    public GameObject enemyPrefab;
    public int secondsToDifficultyIncrease = 30;
    public float initialSecondsBetweenSpawn = 5;
    public float spawnRateIncreasePercentage = 10;
    public float spawnRateRandomRange = 1;

    private void Awake()
    {
        enemyContainer = GameObject.Find("Enemy Container").transform;
        currentSecondsBetweenSpawn = initialSecondsBetweenSpawn;
    }

    public void StartSpawning()
    {
        StopSpawning();

        spawnCoroutine = StartCoroutine(SpawnBehaviour());
        spawnRateIncreaseCoroutine = StartCoroutine(SpawnRateIncreaseBehaviour());
    }

    public void StopSpawning()
    {
        if(spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
        
        if(spawnRateIncreaseCoroutine != null)
        {
            StopCoroutine(spawnRateIncreaseCoroutine);
            spawnRateIncreaseCoroutine = null;
        }
    }

    public void Restart()
    {
        foreach(Transform enemy in enemyContainer)
        {
            Destroy(enemy.gameObject);
        }

        StartSpawning();
    }

    private IEnumerator SpawnBehaviour()
    {
        yield return new WaitForSecondsRealtime(currentSecondsBetweenSpawn + Random.Range(-spawnRateRandomRange, spawnRateRandomRange));
        Instantiate(enemyPrefab, enemyContainer);
        spawnCoroutine = StartCoroutine(SpawnBehaviour());
    }

    private IEnumerator SpawnRateIncreaseBehaviour()
    {
        yield return new WaitForSecondsRealtime(secondsToDifficultyIncrease);
        currentSecondsBetweenSpawn *= (spawnRateIncreasePercentage * 0.01f) + 1f;
        spawnRateIncreaseCoroutine = StartCoroutine(SpawnRateIncreaseBehaviour());
    }
}
