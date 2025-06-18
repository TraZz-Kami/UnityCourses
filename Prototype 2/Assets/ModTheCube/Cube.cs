using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public MeshRenderer Renderer;
    public string playerName;

    private float randomX;
    private float randomY;
    private float randomZ;
    private float randomA;
    private Material material;
    private bool canChange = true;
    
    void Start()
    {
    }
    
    void Update()
    {
        if (canChange)
        {
            canChange = false;
            StartCoroutine(RandomizeCube());
        }
        transform.Rotate(randomX * Time.deltaTime, randomY, randomZ);
    }

    private IEnumerator RandomizeCube()
    {
        randomX = Random.Range(-8.0f, 8.0f);
        randomY = Random.Range(-8.0f, 8.0f);
        randomZ = Random.Range(-8.0f, 8.0f);
        randomA = Random.Range(0.1f, 1.0f);
        
        transform.position = new Vector3(randomX, randomY, randomZ);
        transform.localScale = Vector3.one * randomA;
        
        material = Renderer.material;
        
        material.color = new Color(randomX, randomY, randomZ, randomA);
        
        yield return new WaitForSeconds(2f);
        canChange = true;
    }
}
