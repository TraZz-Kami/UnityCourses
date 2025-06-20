using UnityEngine;
using UnityEngine.UI;

public class DifficultyButton : MonoBehaviour
{
    [SerializeField] private int difficulty;
    
    private Button button;
    private GameManager gameManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(SetDifficulty);
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void SetDifficulty()
    {
        Debug.Log("Difficulty set to " + difficulty);
        gameManager.StartGame(difficulty);
        gameObject.SetActive(false); // Hide the button after setting the difficulty
    }
}
