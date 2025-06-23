using UnityEngine;
using UnityEngine.UI;

public class ButtonDifficulty : MonoBehaviour
{
    [SerializeField] private int difficulty;
    [SerializeField] private Button button;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        button.onClick.AddListener(SetDifficulty);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void SetDifficulty()
    {
        Debug.Log("Difficulty set to " + difficulty);
        GameManager.Instance.StartGame(difficulty);
    }
}
