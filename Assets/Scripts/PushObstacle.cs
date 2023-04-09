using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PushObstacle : MonoBehaviour
{
    public float pushForce;
    public float raycastRange;
    public LayerMask obstacleLayer;


    private void Update()
    {
        RaycastHit hit;
        if (Keyboard.current.fKey.wasPressedThisFrame && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, raycastRange, obstacleLayer))
        {
            Vector3 cameraPos = Camera.main.transform.position;
            Vector3 cameraForward = Camera.main.transform.forward;
            Vector3 forceDirection = cameraForward.normalized;

            hit.rigidbody.AddForce(forceDirection * pushForce, ForceMode.Impulse);
        }
    }
}
