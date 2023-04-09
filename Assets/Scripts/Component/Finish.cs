using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour, IComponent
{
    //public float waitDelay;
    private bool isOpen;

    [Header("Audio")]
    public AudioSource source;
    public AudioClip[] finishGameClips;

    private bool doOnce;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }
    void Update()
    {
        if (isOpen)
        {
            if (!doOnce)
            {
                doOnce = true;
                Debug.Log("Finish Game");
                StartCoroutine(EndGame());
            }
        }
    }

    IEnumerator EndGame()
    {
        var clip = PickRandomTrack(finishGameClips);
        source.PlayOneShot(clip);
        yield return new WaitForSeconds(clip.length);
        FindObjectOfType<LevelLoader>().LoadMainMenuScene();
    }


    public void TriggerComponentTrue()
    {
        isOpen = true;
    }
    public void TriggerComponentFalse()
    {
        isOpen = false;
    }

    private AudioClip PickRandomTrack(AudioClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);
        return clips[randomIndex];
    }
}
