using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    private GameManager gameManager;
    private Transform enemyContainer;
    private Coroutine spawnCoroutine;
    private Coroutine spawnRateIncreaseCoroutine;
    private float currentSecondsBetweenSpawn;
    public GameObject enemyPrefab;
    public int secondsToDifficultyIncrease = 30;
    public int initialSecondsBetweenSpawn = 5;
    public float spawnRateIncreasePercentage = 10;
    public float spawnRateRandomRange = 1;

    private void Awake()
    {
        gameManager = GameObject.FindAnyObjectByType<GameManager>();
        enemyContainer = GameObject.Find("Enemy Container").transform;
        currentSecondsBetweenSpawn = initialSecondsBetweenSpawn;
    }

    private void Start()
    {
        StartSpawning();
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
