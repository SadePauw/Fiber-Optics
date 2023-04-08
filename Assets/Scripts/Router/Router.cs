using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Router : MonoBehaviour, IRouterTasks
{
    public bool isConnected;
    public ElectricalBoxPower connectedFrom;
    public bool hasPower = false;

    public Material connectedMaterial;
    public Material DisconnectedMaterial;

    [Header("Line Renderer")]
    public LineRenderer lineRenderer;


    private Renderer renderer;

    private void Awake()
    {
        renderer = GetComponent<Renderer>();
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
        if (connectedFrom != null)
        {
            hasPower = connectedFrom.hasPower;
        }

        if (connectedFrom != null)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, connectedFrom.transform.position);
        }
        else
        {
            lineRenderer.positionCount = 0;
        }
    }

    public void ClearRouterConnection()
    {
        connectedFrom = null;
    }


    public void SetRouterMaterial(bool isOnline)
    {
        if (isOnline)
        {
            renderer.material = connectedMaterial;
        }
        else
        {
            renderer.material = DisconnectedMaterial;
        }
        isConnected = isOnline;
    }
}
