using System;
using UnityEngine;

// INHERITANCE: Enemy class inherits from Entity class
public class Enemy : Entity
{
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
        if (!GameManager.Instance.IsGameStarted)
        {
            animator.enabled = false;
            return;
        }
        Vector3 target = isPlayerInRange ? player.transform.position : campFire.transform.position;
        Vector3 direction = target - transform.position;
        MoveTo(direction); // ABSTRACTION
        RotateTo(direction); // ABSTRACTION
    }

    private void FixedUpdate()
    {
        if (!GameManager.Instance.IsGameStarted)
            return;

    }
    
    // POLYMORPHISM: override the base class method to implement specific behavior for enemies
    protected override void TakeDamage(int amount, bool isEnemy = false)
    {
        if(isEnemy)
            Instantiate(coinPrefab, gameObject.transform.position, coinPrefab.transform.rotation);

        SpawnManager.Instance.KillEnnemy(gameObject);
        Destroy(gameObject);
    }

    
    private void OnCollisionEnter(Collision other)
    {
        // If enemy collides with fire camp, destroy it
        if (other.gameObject.CompareTag("CampFire"))
        {
            TakeDamage(0);
        }
        else if (other.gameObject.CompareTag("Projectile"))
        {
            TakeDamage(0, true);
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
