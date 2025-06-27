using UnityEngine;

// Base class that demonstrates abstraction
public abstract class Entity : MonoBehaviour
{
    [SerializeField] protected float moveSpeed = 5f;
    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected Animator animator;

    protected Quaternion LookRotation;
    
    // POLYMORPHISM (can be overridden)
    protected virtual void TakeDamage(int amount, bool isEnemy = false) {} 
    
    // POLYMORPHISM (can be overridden)
    protected virtual void Move(Vector3 direction)
    {
        direction.Normalize();
        rb.MovePosition(rb.position + direction * (moveSpeed * Time.deltaTime));
        animator?.SetFloat("Speed_f", direction.magnitude);
    }
    
    // POLYMORPHISM (can be overridden)
    protected virtual void Look(Vector3 direction)
    {
        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
            rb.MoveRotation(lookRotation);
        }
    }

    // ABSTRACTION: high-level method hides the implementation details of movement and rotation
    protected void NavigateTo(Vector3 direction)
    {
        Move(direction); // ABSTRACTION
        Look(direction); // ABSTRACTION
    }
    
    // ABSTRACTION: separates movement and rotation
    public void MoveTo(Vector3 direction) => Move(direction);
    public void RotateTo(Vector3 direction) => Look(direction);

}
