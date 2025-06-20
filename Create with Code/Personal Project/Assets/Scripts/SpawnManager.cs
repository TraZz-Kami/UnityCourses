using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [field: SerializeField] public int EnemyCount { get; private set; }
    [SerializeField] private int waveNumber = 1;

    private float spawnRangeX = 20f;
    private float spawnRangeZ = 20f;
    private bool waveStarted = false;

    private float spawnX;
    private float spawnZ;
    
    // Singleton instance
    public static SpawnManager Instance { get; private set; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        SpawnEnemyWave(waveNumber);
    }

    // Update is called once per frame
    void Update()
    {
        if(EnemyCount == 0 && !waveStarted)
        {
            waveStarted = true; // Prevent multiple waves from starting at the same time
            StartCoroutine(StartWave());
        }
    }
    
    
    private Vector3 GetRandomSpawnPosition()
    {
        // Choose if it will spawn top or right (left or bottom is not used) random boolean
        // This is to ensure that enemies spawn in a more controlled area
        bool spawnTopOrRight = Random.value > 0.5f;

        if (spawnTopOrRight)
        {
            spawnX = Random.Range(-spawnRangeX, spawnRangeX);
            spawnZ = spawnRangeZ;
        }
        else
        {
            spawnZ = Random.Range(-spawnRangeZ, spawnRangeZ);
            spawnX = spawnRangeX;
        }
        
        return new Vector3(spawnX, 2, spawnZ);
    }
    
    private void SpawnEnemyWave(int enemyCount)
    {
        for (int i = 0; i < enemyCount; i++)
        {
            Instantiate(enemyPrefab, GetRandomSpawnPosition(), enemyPrefab.transform.rotation);
        }
        EnemyCount = enemyCount; // Update the enemy count
        waveNumber++;
        waveStarted = false;
    }
    
    private IEnumerator StartWave()
    {
        WaitForSeconds seconds = new WaitForSeconds(2f);
        yield return seconds; // Wait for 2 seconds before spawning the next wave
        SpawnEnemyWave(waveNumber);
    }
    
    public void KillEnnemy(GameObject enemy)
    {
        if (enemy)
        {
            EnemyCount--;
        }
    }
}
