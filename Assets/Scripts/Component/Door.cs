using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class Door : MonoBehaviour, IComponent
{
    public Vector3 positionToMoveTo;
    public float lerpSpeed;


    private bool isOpen;
    private Vector3 defaultPos;

    private void Start()
    {
        defaultPos = transform.position;
    }
    void Update()
    {
        if (isOpen)
        {
            transform.position = Vector3.Lerp(transform.position, defaultPos + positionToMoveTo, lerpSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, defaultPos, lerpSpeed * Time.deltaTime);
        }
    }

    IEnumerator LerpPosition(Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = transform.position;
        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
    }

    public void TriggerComponentTrue()
    {
        isOpen = true;
    }
    public void TriggerComponentFalse()
    {
        isOpen = false;
    }
}
