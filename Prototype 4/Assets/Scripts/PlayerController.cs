using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject focalPoint;
    [SerializeField] private GameObject powerupIndicator;
    
    private float movement;
    private bool hasPowerup = false;
    private float powerupStrength = 15f;
    
    public void OnMove(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>().y;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rb.AddForce(focalPoint.transform.forward * (moveSpeed * movement));
        powerupIndicator.transform.position = transform.position + new Vector3(0, -0.5f, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Powerup"))
        {
            hasPowerup = true;
            Destroy(other.gameObject);
            powerupIndicator.SetActive(true);
            StartCoroutine(PowerupCountdownRoutine());
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy") && hasPowerup)
        {
            Rigidbody rbEnemy = other.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = other.transform.position - transform.position;
            
            rbEnemy.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
        }
    }
    
    private IEnumerator PowerupCountdownRoutine()
    {
        yield return new WaitForSeconds(7);
        hasPowerup = false;
        powerupIndicator.SetActive(false);
    }
}
