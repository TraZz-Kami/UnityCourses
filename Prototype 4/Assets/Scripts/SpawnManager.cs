using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int enemyCount;
    [SerializeField] private int waveNumber = 1;
    [SerializeField] private GameObject powerupPrefab;
    
    private float spawnRange = 9f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnEnemyWave(waveNumber);
        Instantiate(powerupPrefab, GetRandomSpawnPosition(), powerupPrefab.transform.rotation);    
    }

    // Update is called once per frame
    void Update()
    {
        enemyCount = FindObjectsByType<Enemy>(FindObjectsSortMode.None).Length;
        if (enemyCount == 0)
        {
            Instantiate(powerupPrefab, GetRandomSpawnPosition(), powerupPrefab.transform.rotation);
            waveNumber++;
            SpawnEnemyWave(waveNumber);
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float rangeX = Random.Range(-spawnRange, spawnRange);
        float rangeZ = Random.Range(-spawnRange, spawnRange);
        Vector3 spawnPosition = new Vector3(rangeX, 0, rangeZ);
        
        return spawnPosition;
    }

    private void SpawnEnemyWave(int enemyCount)
    {
        for (int i = 0; i < enemyCount; i++)
        {
            Instantiate(enemyPrefab, GetRandomSpawnPosition(), enemyPrefab.transform.rotation);
        }
    }
}
