using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricalBoxPower : MonoBehaviour, IElectricalBoxConnector
{
    public bool hasInfinitePower;

    [Header("Connections")]
    public ElectricalBoxPower connectedFrom;
    public ElectricalBoxPower connectedTo;
    public bool connectsToComponent;

    public Router connectedRouter;

    [Header("Materials")]
    public Renderer powerRenderer;
    public Material powered;
    public Material notPowered;

    [Header("Line Renderer")]
    public LineRenderer lineRenderer;
    public Material poweredMaterial;
    public Material notPoweredMaterial;

    //Cache
    public bool hasPower;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

        if (hasInfinitePower)
        {
            powerRenderer.material = powered;
            hasPower = true;
        }
        else
        {
            powerRenderer.material = notPowered;
        }
    }

    private void Update()
    {
        if (connectedFrom != null && hasPower != connectedFrom.hasPower)
        {
            CheckPower();
        }

        if (connectedTo != null)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, connectedTo.transform.position);
        }

        if (hasPower)
        {
            lineRenderer.material = poweredMaterial;
        }
        else
        {
            lineRenderer.material = notPoweredMaterial;
        }
    }

    public void ConnectToRouter(Router router)
    {
        connectedRouter = router;
    }

    public void SetConnectedBoxFrom(ElectricalBoxPower box)
    {
        if (hasInfinitePower)
        {
            //Debug.Log("Can't connect to a power source.");
            return;
        }
        if (connectedFrom != null) { /*Debug.Log("Box already has connectedFrom")*/; return; }
        connectedFrom = box;
    }

    public void SetConnectedBoxTo(ElectricalBoxPower box)
    {
        if (connectedTo != null || box.hasInfinitePower) { /*Debug.Log("Box already has connectedTo or is a power source")*/; return; }
        connectedTo = box;
    }

    public void SwapFromTo()
    {
        //Debug.Log("swap");
        if (connectedFrom != null && connectedFrom != this) //<-- connectodFrom != this ??
        {
            connectedFrom.SwapFromTo();
        }
        (connectedFrom, connectedTo) = (connectedTo, connectedFrom);
    }

    public void ClearConnections()
    {
        lineRenderer.positionCount = 0;
        if (connectedRouter != null)
        {
            connectedRouter.ClearRouterConnection();
            connectedRouter = null;
        }

        if (connectedFrom != null)
        {
            connectedFrom.lineRenderer.positionCount = 0;
            connectedFrom.ClearConnectTo();
        }

        if (connectedTo != null)
        {
            connectedTo.lineRenderer.positionCount = 0;
            connectedTo.ClearConnectFrom();
        }

        connectedFrom = null;
        connectedTo = null;

        if (hasInfinitePower) { return; }
        hasPower = false;
        powerRenderer.material = notPowered;
    }

    public void ClearConnectTo()
    {
        connectedTo = null;
    }
    public void ClearConnectFrom()
    {
        connectedFrom = null;
        CheckPower();
    }

    private void CheckPower()
    {
        if (connectedFrom != null)
        {
            hasPower = connectedFrom.hasPower;
        }
        if (connectedFrom == null)
        {
            hasPower = false;
        }

        Power();

        void Power()
        {
            if (hasPower)
            {
                powerRenderer.material = powered;
            }
            else
            {
                powerRenderer.material = notPowered;
            }
        }
    }
}
