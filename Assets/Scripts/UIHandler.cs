using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Timeline.AnimationPlayableAsset;

public class UIHandler : MonoBehaviour
{
    public GameObject Menu;
    public GameObject[] otherUIElements;
    private bool menuIsOpen = false;

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame && !menuIsOpen)
        {   
            foreach (var item in otherUIElements)
            {
                item.SetActive(false);
            }
            menuIsOpen = true;
            Cursor.lockState = CursorLockMode.None;
            OpenMenu();
            Time.timeScale = 0;
        }
    }

    private void OpenMenu()
    {
        Menu.SetActive(true);
    }
    public void CloseMenu()
    {
        foreach (var item in otherUIElements)
        {
            item.SetActive(true);
        }
        menuIsOpen = false;
        Cursor.lockState = CursorLockMode.Locked;
        Menu.SetActive(false);
        Time.timeScale = 1;
    }
}
