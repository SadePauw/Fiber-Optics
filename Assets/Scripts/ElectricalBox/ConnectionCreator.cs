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
    public ElectricalComponent nearestComponent;
    public LineRenderer lineRenderer;
    public Vector3 lineOffset;
    public float maxWireLength;
    public float distFromCurrentBox;

    [Header("Path Obstruction")]
    public LayerMask obstacleLayers;

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

        if (other.gameObject.layer == LayerMask.NameToLayer("Component"))
        {
            nearestComponent = other.gameObject.GetComponent<ElectricalComponent>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        nearestBox = null;
        nearestRouter = null;
        nearestComponent = null;
    }

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
    }
    private void Update()
    {
        if (currentBox != null)
        {
            distFromCurrentBox = Vector3.Distance(transform.position, currentBox.transform.position);
        }
        ButtonPressed();
        UpdateUiImageColor();
        DrawLineToHand();
    }

    private void ButtonPressed()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame && nearestBox != null)
        {
            if (currentBox == null)
            {
                if (nearestBox.connectedFrom != null && nearestBox.connectedTo != null || nearestBox.connectsToComponent) { return; }
                UpdateUIText("Picked up wire");
                PickUpConnection(nearestBox);
            }
            else if (currentBox != null && currentBox != nearestBox && nearestBox != null && !nearestBox.hasPower)
            {
                if (distFromCurrentBox >= maxWireLength) { UpdateUIText("Can't connect, to far away!"); return; }
                if (IsPathObstructed(nearestBox.transform.position, currentBox.transform.position, obstacleLayers)) { UpdateUIText("Path is Obstructed"); return; }
                UpdateUIText("Box Connected");
                if (!nearestBox.hasPower && currentBox.hasPower && nearestBox.connectedFrom != null)
                {
                    nearestBox.SwapFromTo();
                }
                nearestBox.GetComponent<IElectricalBoxConnector>().SetConnectedBoxFrom(currentBox);
                currentBox.GetComponent<IElectricalBoxConnector>().SetConnectedBoxTo(nearestBox);
                currentBox = null;
            }
            else if (nearestBox == currentBox)
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
                if (distFromCurrentBox >= maxWireLength) { UpdateUIText("Can't connect, to far away!"); return; }
                UpdateUIText("Connected Router");
                nearestRouter.connectedFrom = currentBox;
                currentBox.connectsToComponent = true;
                currentBox = null;
            }
            else if (nearestBox == null && nearestRouter.connectedFrom != null)
            {
                UpdateUIText("Cleared Router Connection");
                nearestRouter.connectedFrom.connectsToComponent = false;
                nearestRouter.GetComponent<IRouterTasks>().ClearRouterConnection();
            }
        }
        else if (Keyboard.current.eKey.wasPressedThisFrame && nearestComponent != null)
        {
            if (currentBox != null)
            {
                if (distFromCurrentBox >= maxWireLength) { UpdateUIText("Can't connect, to far away!"); return; }
                UpdateUIText("Connected Component");
                nearestComponent.connectedFrom = currentBox;
                currentBox.connectsToComponent = true;
                currentBox = null;
            }
            else if (nearestBox == null && nearestComponent.connectedFrom != null)
            {
                UpdateUIText("Cleared Component Connection");
                nearestComponent.connectedFrom.connectsToComponent = false;
                nearestComponent.GetComponent<IRouterTasks>().ClearRouterConnection(); //Component uses the IRouterTask since I didn't want to create a seperate Interface for it.
            }
        }
        else if (Keyboard.current.eKey.wasPressedThisFrame && currentBox != null)
        {
            currentBox = null;
            UpdateUIText("Let go of wire");
        }
    }

    public bool IsPathObstructed(Vector3 pointA, Vector3 pointB, LayerMask mask)
    {
        Vector3 direction = pointB - pointA;
        RaycastHit hit;

        if (Physics.Raycast(pointA, direction.normalized, out hit, direction.magnitude, mask))
        {
            // If the raycast hit something, then the path is obstructed
            return true;
        }

        // Otherwise, the path is not obstructed
        return false;
    }

    private void DrawLineToHand()
    {
        if (currentBox != null)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, transform.position + lineOffset);
            lineRenderer.SetPosition(1, currentBox.transform.position);
        }
        else
        {
            lineRenderer.positionCount = 0;
        }
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
