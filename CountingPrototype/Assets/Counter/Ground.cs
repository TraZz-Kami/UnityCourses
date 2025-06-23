using System;
using UnityEngine;

public class Ground : MonoBehaviour
{
    [SerializeField] private SpawnManager spawnManager;
    private int health = 10;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Raindrop"))
        {
            health--;
            Destroy(other.gameObject);
            GameManager.Instance.UpdatePlayerHealthText(health);
            spawnManager.KillEnnemy(other.gameObject);
            
            if(health <= 0)
            {
                GameManager.Instance.GameOver();
            }
        }
    }
}
