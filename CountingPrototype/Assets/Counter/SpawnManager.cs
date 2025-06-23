using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [field: SerializeField] public int EnemyCount { get; private set; }
    [SerializeField] private int waveNumber = 1;

    private float spawnRangeZ = 17f;
    private bool waveStarted = false;

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
    }

    // Update is called once per frame
    void Update()
    {
        if(EnemyCount <= 0 && !waveStarted && GameManager.Instance.IsGameStarted)
        {
            waveStarted = true; // Prevent multiple waves from starting at the same time
            StartCoroutine(StartWave());
        }
    }
    
    
    private Vector3 GetRandomSpawnPosition()
    {
        // Choose if it will spawn top or right (left or bottom is not used) random boolean
        // This is to ensure that enemies spawn in a more controlled area
        
            spawnZ = Random.Range(-spawnRangeZ, spawnRangeZ);
        
        return new Vector3(0, 18, spawnZ);
    }
    
    private IEnumerator StartWave()
    {
        WaitForSeconds seconds = new WaitForSeconds(2f);
        yield return seconds; // Wait for 2 seconds before spawning the next wave
        
        for (int i = 0; i < waveNumber; i++)
        {
            if(!GameManager.Instance.IsGameStarted)
                yield break; // Exit if the game is not started
            Instantiate(enemyPrefab, GetRandomSpawnPosition(), enemyPrefab.transform.rotation);
            float spawnDelay = Random.Range(0.5f, 1.5f);
            yield return new WaitForSeconds(spawnDelay);
        }
        EnemyCount = waveNumber; // Update the enemy count
        waveNumber++;
        waveStarted = false;        
        
       
    }
    
    public void KillEnnemy(GameObject enemy)
    {
        if (enemy)
        {
            EnemyCount--;
        }
    }

    public void SetDifficulty(int difficulty)
    {
        waveNumber = difficulty;
        StartCoroutine(StartWave());
    }
}
