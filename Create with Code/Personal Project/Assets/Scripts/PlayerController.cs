using System;
using System.Collections;
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
    private int health = 10;

    public void OnMove(InputAction.CallbackContext context)
    {

        moveInput = context.ReadValue<Vector2>();
        // Debug.Log(moveInput.x +" "+ moveInput.y);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed)
            return;

        var device = context.control.device;
        Vector2 val = context.ReadValue<Vector2>();
        // Debug.Log($"OnLook: {device.GetType().Name} → {val}");

        // Traiter sticks Gamepad OU Hatswitch Joystick
        if (device is Gamepad || device is Joystick)
        {
            if (val.sqrMagnitude > 0.01f)
            {
                isGamepad = true;
                lookInput = val;
            }
        }
        // Traiter la souris
        else if (device is Pointer)
        {
            isGamepad = false;
            // lookInput reste inchangé, on utilisera raycast sur Mouse.current.position
        }
    }

    private void Start()
    {
        coins = 0;
        health = 10;
    }

    private void FixedUpdate()
    {
        if (!GameManager.Instance.IsGameStarted)
            return;
        
        
        // Update UI with player stats
        GameManager.Instance.UpdatePlayerText(health, coins);

        LookAtPointer();
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (moveInput.sqrMagnitude > 0f)
        {
            Vector3 movement = new Vector3(moveInput.x, 0f, moveInput.y);
            movement = Quaternion.Euler(0, 45, 0) * movement;
            movement.Normalize();
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
        if (isGamepad)
        {
            lookRotation = Quaternion.LookRotation(new Vector3(lookInput.x, 0f, lookInput.y));
        }
        else
        {
            Plane ground = new Plane(Vector3.up, transform.position);
            Ray ray = camera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (ground.Raycast(ray, out float enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);
                lookRotation = Quaternion.LookRotation(hitPoint - transform.position);
            }
        }

        rb.MoveRotation(lookRotation);

    }


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            animator.SetInteger(1, 1);
            health--;
            GameManager.Instance.UpdatePlayerText(health, coins);
            if (health <= 0)
            {
                // Handle player death
                Debug.Log("Player has died.");
                animator.SetBool("Dead_b", true);
                animator.enabled = false;
                GameManager.Instance.GameOver();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("CampFire"))
        {
            StartCoroutine(HealPlayer());
        }
        else if (other.gameObject.CompareTag("TowerBase"))
        {
            if (coins >= 3)
            {
                coins -= 3;
                other.gameObject.SetActive(false);
                Tower.CreateTower(towerPrefab, other.transform.position, towerPrefab.transform.rotation);
                GameManager.Instance.DesactivateTowerCoinText();
            }
            else
            {
                GameManager.Instance.ActivateTowerCoinText(3 - coins);
            }
        }
        else if (other.gameObject.CompareTag("Coin"))
        {
            Destroy(other.gameObject);
            coins++;
            GameManager.Instance.UpdatePlayerText(health, coins);
        }
    }

    private IEnumerator HealPlayer()
    {
        if(health < 10)
        {
            health++;
            GameManager.Instance.UpdatePlayerText(health, coins);
            yield return new WaitForSeconds(1f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("TowerBase"))
        {
            GameManager.Instance.DesactivateTowerCoinText();
        }
    }
}