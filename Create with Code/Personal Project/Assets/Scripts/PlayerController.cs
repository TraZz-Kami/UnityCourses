using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

// INHERITANCE: PlayerController inherits from Entity
public class PlayerController : Entity
{
    [SerializeField] private Camera cameraPlayer;
    [SerializeField] private GameObject towerPrefab;

    private Vector2 moveInput;
    private Vector2 lookInput;
    private bool isGamepad;
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

        // ABSTRACTION: call the base class method to handle movement and look
        RotateTo(new Vector3(lookInput.x, 0f, lookInput.y));
        MoveTo(new Vector3(moveInput.x, 0f, moveInput.y));
    }

    // POLYMORPHISM: override the base class method to implement specific behavior for player
    protected override void Move(Vector3 movement)
    {
        if (moveInput.sqrMagnitude > 0f)
        {
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

    // POLYMORPHISM: override the base class method to implement specific behavior for player
    protected override void Look(Vector3 lookDirection)
    {
        if (isGamepad)
        {
            LookRotation = Quaternion.LookRotation(lookDirection);
        }
        else
        {
            Plane ground = new Plane(Vector3.up, transform.position);
            Ray ray = cameraPlayer.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (ground.Raycast(ray, out float enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);
                LookRotation = Quaternion.LookRotation(hitPoint - transform.position);
            }
        }

        rb.MoveRotation(LookRotation);

    }


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(1);
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
    
    // POLYMORPHISM: override the base class method to implement specific behavior for player
    protected override void TakeDamage(int amount, bool isEnemy = false)
    {            
        animator.SetInteger(1, 1);
        health -= amount;
        GameManager.Instance.UpdatePlayerText(health, coins);
    
        if (health <= 0)
        {
            animator.SetBool("Dead_b", true);
            animator.enabled = false;
            GameManager.Instance.GameOver();
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