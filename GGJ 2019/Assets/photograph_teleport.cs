using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class photograph_teleport : pickupable
{
    [SerializeField]
    int_var creepiness;
    [SerializeField]
    Transform teleport_point;
    private Vector3 startPoint;

    private void Start()
    {
        startPoint = transform.position;
    }

    protected override void OnPickup()
    {
        if (firstPickup)
        {
            Debug.Log("AAAA");
            creepiness.val++;
            
        }
    }

    protected override void OnLetGo()
    {
        GetComponent<Renderer>().enabled = false;
        Transform pTrans = GameObject.FindGameObjectWithTag("Player").transform;
        Vector3 offset = pTrans.position - startPoint;
        pTrans.position = teleport_point.position + offset;
    }
}

