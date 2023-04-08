using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using System;

public class ConnectionCreator : MonoBehaviour
{
    public ElectricalBoxPower currentBox;
    public ElectricalBoxPower nearestBox;
    public Router nearestRouter;

    [Header("UI")]
    public Image hasWireImage;
    public TextMeshProUGUI descriptionText;


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("ElectricalBox"))
        {
            nearestBox = other.gameObject.GetComponent<ElectricalBoxPower>();
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Router"))
        {
            nearestRouter = other.gameObject.GetComponent<Router>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        nearestBox = null;
        nearestRouter = null;
    }

    private void Start()
    {
    }
    private void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame && nearestBox != null)
        {
            if (currentBox == null)
            {
                UpdateUIText("Picked up wire");
                PickUpConnection(nearestBox);
            }
            else if (currentBox != null && currentBox != nearestBox && nearestBox != null)
            {
                UpdateUIText("Box Connected");
                if (!nearestBox.hasPower && currentBox.hasPower && nearestBox.connectedFrom != null)
                {
                    Debug.Log("Check Swap");
                    nearestBox.SwapFromTo();
                }
                nearestBox.GetComponent<IElectricalBoxConnector>().SetConnectedBoxFrom(currentBox);
                currentBox.GetComponent<IElectricalBoxConnector>().SetConnectedBoxTo(nearestBox);

                currentBox = null;
            }
            else if(nearestBox == currentBox)
            {
                nearestBox.GetComponent<IElectricalBoxConnector>().ClearConnections();
                UpdateUIText("Connections Cleared");
                currentBox = null;
            }
            else
            {
                currentBox = null;
                UpdateUIText("Let go of wire");
            }
        }
        else if (Keyboard.current.eKey.wasPressedThisFrame && nearestRouter != null)
        {
            if (currentBox != null)
            {
                UpdateUIText("Connected Router");
                nearestRouter.connectedFrom = currentBox;
            }
            if (nearestBox != null && nearestRouter.connectedFrom != null)
            {
                UpdateUIText("Cleared Router Connection");
                nearestRouter.GetComponent<IRouterTasks>().ClearRouterConnection();
            }
        }

        UpdateUiImageColor();
    }

    private void UpdateUIText(string desc)
    {
        descriptionText.text = desc;
    }

    private void UpdateUiImageColor()
    {
        if (currentBox != null)
        {
            hasWireImage.color = Color.green;
        }
        else
        {
            hasWireImage.color = Color.red;
        }
    }

    private void PickUpConnection(ElectricalBoxPower box)
    {
        currentBox = box;
    }
}
