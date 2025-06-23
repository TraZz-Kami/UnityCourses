using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [SerializeField] private TextMeshProUGUI coinCountText;
    [SerializeField] private TextMeshProUGUI healthCountText;
    [SerializeField] private GameObject GameOverPanel;
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private SpawnManager spawnManager;
    
    public Component GetCo

    public bool IsGameStarted { get; private set; }
    private float spawnRate = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        IsGameStarted = false;
        GameOverPanel.gameObject.SetActive(false);
        mainMenuPanel.SetActive(true);
        gamePanel.SetActive(false);
        
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

    public void UpdatePlayerText(int coins)
    {
        coinCountText.text = $"Raindrops: {coins}";
    }
    public void UpdatePlayerHealthText(int health)
    {
        healthCountText.text = $"Lives: {health}";
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
        UpdatePlayerText(0);
        UpdatePlayerHealthText(10);
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
