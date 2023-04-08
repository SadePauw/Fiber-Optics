using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public int mainMenuIndex;
    public int gameSceneIndex;

    //Cache
    int currentSceneIndex;

    void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene(gameSceneIndex);
    }

    public void LoadMainMenuScene()
    {
        SceneManager.LoadScene(mainMenuIndex);
    }
}
