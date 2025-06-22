using Unity.VisualScripting;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float speed;

    private void LateUpdate()
    {
       // Translate the iso camera to the target position with offset
         if (target != null)
         {
              Vector3 desiredPosition = target.position + offset;
              transform.position = desiredPosition;
         }
         else
         {
              Debug.LogWarning("Target is not assigned in CameraFollow script.");
         }
    }
}
