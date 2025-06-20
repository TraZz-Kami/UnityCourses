using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private int pointValue = 5; // Points awarded for hitting the target
    [SerializeField] private ParticleSystem explosionParticle;
    
    private Rigidbody targetRb;
    
    private float minSpeed = 12f;
    private float maxSpeed = 16f;
    private float maxTorque = 10f;
    private float xRange = 4f;
    private float ySpawnPos = -6f;
    
    private GameManager gameManager;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetRb = GetComponent<Rigidbody>();
        targetRb.AddForce(RandomForce(), ForceMode.Impulse);
        targetRb.AddTorque(RandomTorque(), RandomTorque(), RandomTorque(), ForceMode.Impulse);
        transform.position = RandomSpawnPos();
        
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private Vector3 RandomForce()
    {
        return Vector3.up * Random.Range(minSpeed, maxSpeed);
    }

    private float RandomTorque()
    {
        return Random.Range(-maxTorque, maxTorque);
    }

    private Vector3 RandomSpawnPos()
    {
        return new Vector3(Random.Range(-xRange, xRange), ySpawnPos);
    }
    
    private void OnMouseDown()
    {
        if(gameManager.isGameActive == false) return;
        Destroy(gameObject);
        Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);
        gameManager.UpdateScore(pointValue);
    }
    
    private void OnTriggerEnter(Collider other)
    {

            Destroy(other.gameObject);
            if (!gameObject.CompareTag("Bad")) gameManager.GameOver();
    }
}
