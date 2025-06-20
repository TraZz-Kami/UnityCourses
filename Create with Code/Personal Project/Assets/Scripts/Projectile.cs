using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, 2f);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Projectile"))
            return;
        Destroy(gameObject);
    }
    
}
