using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Unity.VisualScripting.Member;

public class PushObstacle : MonoBehaviour
{
    public float pushForce;
    public float raycastRange;
    public LayerMask obstacleLayer;

    [Header("Audio")]
    public AudioSource source;
    public AudioClip[] kickClips;


    private void Start()
    {
        source = GetComponent<AudioSource>();
    }
    private void Update()
    {
        RaycastHit hit;
        if (Keyboard.current.fKey.wasPressedThisFrame && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, raycastRange, obstacleLayer))
        {
            PlayConnectClip();
            Vector3 cameraPos = Camera.main.transform.position;
            Vector3 cameraForward = Camera.main.transform.forward;
            Vector3 forceDirection = cameraForward.normalized;

            hit.rigidbody.AddForce(forceDirection * pushForce, ForceMode.Impulse);
        }
    }

    public void PlayConnectClip()
    {
        source.pitch = Random.Range(0.8f, 1.2f);
        source.PlayOneShot(PickRandomTrack(kickClips));
    }

    private AudioClip PickRandomTrack(AudioClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);
        return clips[randomIndex];
    }
}
