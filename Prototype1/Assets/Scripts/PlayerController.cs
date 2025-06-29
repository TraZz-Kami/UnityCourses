using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    [SerializeField] private float speed = 20f;
    [SerializeField] private float turnSpeed= 45.0f;
    [SerializeField] private float horizontalInput;
    [SerializeField] private float forwardInput;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        forwardInput = Input.GetAxis("Vertical");
        
        transform.Translate(Vector3.forward * (Time.deltaTime * speed * forwardInput));
        transform.Rotate(Vector3.up, (Time.deltaTime * turnSpeed * horizontalInput));
    }
}
