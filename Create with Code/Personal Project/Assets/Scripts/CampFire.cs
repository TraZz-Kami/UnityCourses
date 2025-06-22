using System;
using UnityEngine;

public class CampFire : MonoBehaviour
{
    [SerializeField] private int health = 5;
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
        if (other.gameObject.CompareTag("Enemy"))
        {
            health--;
            GameManager.Instance.UpdateCampfireText(health);
            if(health <= 0)
            {
                GameManager.Instance.GameOver();
            }
        }
    }
}
