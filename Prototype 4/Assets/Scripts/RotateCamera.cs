using UnityEngine;
using UnityEngine.InputSystem;

public class RotateCamera : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private InputActionReference rotateAction;
    
    private float rotation;

    // public void OnMove(InputAction.CallbackContext context)
    // {
    //     rotation = context.ReadValue<Vector2>().x;
    // }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rotation = rotateAction.action.ReadValue<Vector2>().x;
        transform.Rotate(Vector3.up, -rotation * rotationSpeed * Time.deltaTime);
    }
}
