using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioHandler : MonoBehaviour
{
    public AudioSource source;
    public AudioClip[] mainMenuTracks;
    public AudioClip[] gameSceneTracks;

    private void OnEnable()
    {
        Scene currentScene= SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        if (sceneName == "MainMenu")
        {
            source.clip = PickRandomTrack(mainMenuTracks);
            source.Play();
        }
        else if (sceneName == "Playground")
        {
            source.clip = PickRandomTrack(gameSceneTracks);
            source.Play();
        }
    }

    private AudioClip PickRandomTrack(AudioClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);
        return clips[randomIndex];
    }
}
