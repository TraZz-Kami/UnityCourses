using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float detectionRange = 5f; 
    [SerializeField] private SphereCollider sphereCollider;
    [SerializeField] private GameObject coinPrefab;
    
    private GameObject campFire;
    private GameObject player;
    private bool isPlayerInRange = false;
    private Vector3 lookDirection;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        campFire = GameObject.FindGameObjectWithTag("CampFire");
        player = GameObject.FindGameObjectWithTag("Player");
        sphereCollider.radius = detectionRange;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerInRange)
            lookDirection = (player.transform.position - transform.position).normalized;
        else 
            lookDirection = new Vector3(campFire.transform.position.x - transform.position.x, 0, campFire.transform.position.z - transform.position.z);
            
        lookDirection.Normalize();
        
        rb.MovePosition(rb.position + lookDirection * (moveSpeed * Time.deltaTime));
    }
    
    private void OnCollisionEnter(Collision other)
    {
        // If enemy collides with fire camp, destroy it
        if (other.gameObject.CompareTag("CampFire"))
        {
            SpawnManager.Instance.KillEnnemy(gameObject);
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Projectile"))
        {
            Instantiate(coinPrefab, gameObject.transform.position, coinPrefab.transform.rotation);
            SpawnManager.Instance.KillEnnemy(gameObject);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }
}
