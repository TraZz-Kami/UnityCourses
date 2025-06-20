using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private GameObject ProjectilePrefab;
    [SerializeField] private Transform FirePoint;
    [SerializeField] private float FireRate = 1f;
    [SerializeField] private float ProjectileSpeed = 10f;
    [SerializeField] private float Range = 15f;
    
    private List<GameObject> ennemies = new();
    private SphereCollider rangeCollider;
    private bool isFiring = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rangeCollider = GetComponent<SphereCollider>();
        rangeCollider.radius = Range;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (!ennemies.Contains(other.gameObject))
                ennemies.Add(other.gameObject);
            Debug.Log(ennemies.Count);

            if (!isFiring)
            {
                isFiring = true;
                StartCoroutine(FireAtEnemies());
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            ennemies.Remove(other.gameObject);
            Debug.Log(ennemies.Count);
        }
    }
    
    private void FireProjectile(GameObject target)
    {
        if (!target)
        {
            ennemies.Remove(target);
            return;
        }
        Vector3 direction = (target.transform.position - FirePoint.position).normalized;
        GameObject projectile = Instantiate(ProjectilePrefab, FirePoint.position, ProjectilePrefab.transform.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.linearVelocity = direction * ProjectileSpeed;
    }
    
    private IEnumerator FireAtEnemies()
    {
        while (ennemies.Count > 0)
        {
            GameObject[] ennemiesCopy = ennemies.ToArray();

            foreach (GameObject ennemy in ennemiesCopy)
            {
                FireProjectile(ennemy);
            }
            
            yield return new WaitForSeconds(FireRate);
        }
        
        isFiring = false;
    }
    
    public static void CreateTower(GameObject tower ,Vector3 position, Quaternion rotation)
    {
        position.y = tower.transform.position.y;
        Instantiate(tower, position, rotation);
    }
}
