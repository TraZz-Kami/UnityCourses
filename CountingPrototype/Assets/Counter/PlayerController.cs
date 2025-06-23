using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private SpawnManager spawnManager;

    private Vector2 moveInput;
    private int raindrops;

    public void OnMove(InputAction.CallbackContext context)
    {

        moveInput = context.ReadValue<Vector2>();
        // Debug.Log(moveInput.x +" "+ moveInput.y);
    }
   

    private void Start()
    {
        raindrops = 0;
    }

    private void FixedUpdate()
    {
        if (!GameManager.Instance.IsGameStarted)
            return;
        
        
        // Update UI with player stats
        GameManager.Instance.UpdatePlayerText(raindrops);

        HandleMovement();
    }

    private void HandleMovement()
    {
        if (moveInput.sqrMagnitude > 0f)
        {
            Vector3 movement = new Vector3(0f, 0f, moveInput.x);
            rb.MovePosition(rb.position + movement * (moveSpeed * Time.deltaTime));
        }
        else
        {
            rb.linearVelocity = Vector3.zero;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Raindrop"))
        {
            raindrops++;
            Destroy(other.gameObject);
            GameManager.Instance.UpdatePlayerText(raindrops);
            spawnManager.KillEnnemy(other.gameObject);
        }
    }

    // private void OnCollisionEnter(Collision other)
    // {
    //     if (other.gameObject.CompareTag("Enemy"))
    //     {
    //         animator.SetInteger(1, 1);
    //         health--;
    //         GameManager.Instance.UpdatePlayerText(health, raindrops);
    //         if (health <= 0)
    //         {
    //             // Handle player death
    //             Debug.Log("Player has died.");
    //             animator.SetBool("Dead_b", true);
    //             animator.enabled = false;
    //             GameManager.Instance.GameOver();
    //         }
    //     }
    // }
}