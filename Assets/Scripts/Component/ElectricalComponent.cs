using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ElectricalComponent : MonoBehaviour, IRouterTasks
{
    public bool _isOnline;
    public ElectricalBoxPower connectedFrom;
    public bool hasPower = false;
    public GameObject componentToSwitch;

    public Material connectedMaterial;
    public Material disconnectedMaterial;

    [Header("Line Renderer")]
    public LineRenderer lineRenderer;
    public Vector3 offset;
    public Material poweredMaterial;
    public Material notPoweredMaterial;


    private Renderer myRenderer;
    private bool doOnce;

    private void Awake()
    {
        myRenderer = GetComponent<Renderer>();
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
            lineRenderer.SetPosition(0, transform.position + offset);
            lineRenderer.SetPosition(1, connectedFrom.transform.position);
        }
        else
        {
            lineRenderer.positionCount = 0;
        }

        if (hasPower)
        {
            lineRenderer.material = poweredMaterial;
        }
        else
        {
            lineRenderer.material = notPoweredMaterial;
        }

        Debug.Log("Power: " + hasPower);
        TriggerComponentBool();
        SetRouterMaterial(hasPower);
    }

    public void TriggerComponentBool()
    {
        if(hasPower && !doOnce)
        {
            doOnce = true;
            Debug.Log("Component triggered on");
            componentToSwitch.GetComponent<IComponent>().TriggerComponentTrue();
        }
        else if (!hasPower && doOnce)
        {
            doOnce = false;
            Debug.Log("Component triggered off");
            componentToSwitch.GetComponent<IComponent>().TriggerComponentFalse();
        }
    }


    public void ClearRouterConnection()
    {
        connectedFrom = null;
        hasPower = false;
    }


    public void SetRouterMaterial(bool isOnline)
    {
        if (isOnline)
        {
            myRenderer.material = connectedMaterial;
        }
        else
        {
            if (!hasPower)
            {
                myRenderer.material = disconnectedMaterial;
            }
        }
    }
}
