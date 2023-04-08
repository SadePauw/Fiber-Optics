using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class WifiConnection : MonoBehaviour
{
    public Image noiseImage;
    private Color noiseColor;

    public float minDistanceFromRouter;
    public float maxDistanceFromRouter;

    public Router[] routers;
    public Slider signalSlider;

    [Header("Audio")]
    public AudioClip[] sipClips;


    [Header("Read Only")]
    public float distToClosestRouter;
    public float waitForEnd;

    [Header("Testing")]

    //Cache
    public float distToClosestRouterNormalized;
    private bool endGame = false;
    private AudioSource source;


    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }
    private void Start()
    {
        signalSlider.value = distToClosestRouterNormalized;
        noiseColor = noiseImage.color;
        SetNoiseAlpha(0);
    }

    private void Update()
    {
        SetNoiseAlphaToClostestRouterDistance();
        signalSlider.value = GetNormalizedDistance(2, maxDistanceFromRouter);
        EndGame();
    }

    private void EndGame()
    {
        if (distToClosestRouter >= maxDistanceFromRouter && !endGame)
        {
            StartCoroutine(EndRoutine());
        }
    }

    private IEnumerator EndRoutine()
    {
        endGame = true;
        source.PlayOneShot(sipClips[Random.Range(0, sipClips.Length)]);
        yield return new WaitForSeconds(waitForEnd);
        FindObjectOfType<LevelLoader>().RestartScene();
    }

    private void SetNoiseAlphaToClostestRouterDistance()
    {
        distToClosestRouter = Vector3.Distance(transform.position, GetClosestRouter().transform.position);
        distToClosestRouterNormalized = GetNormalizedDistance(minDistanceFromRouter, maxDistanceFromRouter);
        SetNoiseAlpha(distToClosestRouterNormalized);
    }
    private void SetNoiseAlpha(float value)
    {
        noiseColor.a = value;
        noiseImage.color = noiseColor;
    }

    private float GetNormalizedDistance(float minDist, float maxDist)
    {
        float dist;
        dist = Mathf.InverseLerp(minDist, maxDist, distToClosestRouter);
        return dist;
    }
    private GameObject GetClosestRouter()
    {
        Router closestRouter = null;
        float distance = 99999;
        foreach (var router in routers)
        {
            router.GetComponent<IRouterTasks>().SetRouterMaterial(false); //<-- Fix this
            if (!router.hasPower) { continue; }
            float dist;
            dist = Vector3.Distance(transform.position, router.transform.position);
            if (dist < distance)
            {
                distance = dist;
                closestRouter = router;
            }
        }
        closestRouter.GetComponent<IRouterTasks>().SetRouterMaterial(true);
        return closestRouter.gameObject;
    }
}
