using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Camera camera;
    [SerializeField] private GameObject towerPrefab;
    [SerializeField] private Animator animator;
    
    private Vector2 moveInput;
    private Vector2 lookInput;
    private bool isGamepad;
    private Quaternion lookRotation;
    private int coins;

    public void OnMove(InputAction.CallbackContext context)
    {
        
        moveInput = context.ReadValue<Vector2>();
        // Debug.Log(moveInput.x +" "+ moveInput.y);
    }
    
    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
        isGamepad = context.control.device == Gamepad.current;

        // Debug.Log(lookInput.x +" "+ lookInput.y);
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
       
        LookAtPointer();
        
        if (moveInput != Vector2.zero)
        {
            Vector3 movement = new Vector3(moveInput.x, 0f, moveInput.y);
        
            //movement trigo to iso
            movement = Quaternion.Euler(0, 45, 0) * movement;
            
            //normalize the movement vector
            movement.Normalize();
           
            // Apply movement and rotation with move
            rb.MovePosition(rb.position + movement * (moveSpeed * Time.deltaTime));
            
            animator.SetFloat("Speed_f", movement.magnitude);
        }
        else
        {
            rb.linearVelocity = Vector3.zero;
            animator.SetFloat("Speed_f", 0);
        }
    }
    
    private void LookAtPointer()
    {
        if (!isGamepad)
        {
            Ray ray = camera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3 targetPosition = hit.point;
                targetPosition.y = transform.position.y;
                lookRotation = Quaternion.LookRotation(targetPosition - transform.position);
            }
        }
        else
        {
            lookRotation = Quaternion.LookRotation(new Vector3(lookInput.x, 0, lookInput.y));
        }
        rb.MoveRotation(lookRotation);
    }

    private void OnCollisionEnter(Collision other)
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("CampFire"))
        {
            //
        }
        else if (other.gameObject.CompareTag("TowerBase"))
        {
            if (coins >= 3)
            {
                coins -= 3;
                other.gameObject.SetActive(false);
                Tower.CreateTower(towerPrefab, other.transform.position, towerPrefab.transform.rotation);
            }
        }
        else if (other.gameObject.CompareTag("Coin"))
        {
            Destroy(other.gameObject);
            coins++;
        }
    }
}
