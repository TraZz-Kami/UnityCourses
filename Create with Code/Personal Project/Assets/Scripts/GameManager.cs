using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [SerializeField] private TextMeshProUGUI enemyCountText;
    [SerializeField] private TextMeshProUGUI coinCountText;
    [SerializeField] private TextMeshProUGUI healthCountText;
    [SerializeField] private TextMeshProUGUI campFireHealthCountText;
    [SerializeField] private TextMeshProUGUI waveCountText;
    [SerializeField] private TextMeshProUGUI nextWaveText;
    [SerializeField] private TextMeshProUGUI towerCoinText;
    [SerializeField] private GameObject GameOverPanel;
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private SpawnManager spawnManager;

    // ENCAPSULATION: The IsGameStarted property is private set to prevent external modification
    public bool IsGameStarted { get; private set; }
    private float spawnRate = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        IsGameStarted = false;
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdatePlayerText(int health, int coins)
    {
        coinCountText.text = $"Coins: {coins}";
        healthCountText.text = $"Health: {health}";
    }
    
    public void UpdateEnemyText(int enemyCount, int waveNumber)
    {
        enemyCountText.text = $"Enemies: {enemyCount}";
        waveCountText.text = $"Wave: {waveNumber}";
    }
    
    public void UpdateCampfireText(int health)
    {
        campFireHealthCountText.text = $"Campfire Health: {health}";
    }

    public IEnumerator UpdateNextWaveText(int waveNumber, int secondsToNextWave)
    {
        nextWaveText.gameObject.SetActive(true);
        if (secondsToNextWave > 0)
        {
            nextWaveText.text = $"Next Wave: {waveNumber} in {secondsToNextWave} seconds";
        }
        else
        {
            nextWaveText.text = $"Next Wave: {waveNumber} coming.";
            yield return new WaitForSeconds(2f);
            nextWaveText.gameObject.SetActive(false);
        }
    }
    
    public void ActivateTowerCoinText(int coins)
    {
        towerCoinText.gameObject.SetActive(true);
        towerCoinText.text = $"You need {coins} coins to build a tower";
    }
    
    public void DesactivateTowerCoinText()
    {
        towerCoinText.gameObject.SetActive(false);
    }

    public void GameOver()
    {
        IsGameStarted = false;
        GameOverPanel.gameObject.SetActive(true);
    }
    
    public void StartGame(int difficulty)
    {
        IsGameStarted = true;
        gamePanel.SetActive(true);
        mainMenuPanel.SetActive(false);
        GameOverPanel.gameObject.SetActive(false);
        UpdateCampfireText(5);
        UpdatePlayerText(10, 0);
        UpdateEnemyText(0, 1);
        nextWaveText.gameObject.SetActive(false);
        spawnManager.SetDifficulty(difficulty);
    }
    
    public void RestartGame()
    {
        IsGameStarted = false;
        gamePanel.SetActive(false);
        mainMenuPanel.SetActive(true);
        GameOverPanel.gameObject.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game is quitting");
    }
}
