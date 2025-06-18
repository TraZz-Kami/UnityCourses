using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    public GameObject dogPrefab;

    private bool canCallDog = true;

    // Update is called once per frame
    void Update()
    {
        // On spacebar press, send dog if you can
        if (Input.GetKeyDown(KeyCode.Space) && canCallDog)
        {
            canCallDog = false;
            StartCoroutine(CallDog());
        }
    }

    //IEnumerator CallDog()
    private IEnumerator CallDog()
    {
        Instantiate(dogPrefab, transform.position, dogPrefab.transform.rotation);
     
        WaitForSeconds wait = new WaitForSeconds(2.0f);
        // Wait 2s in the coroutine
        yield return wait;
        canCallDog = true;
    }
}
