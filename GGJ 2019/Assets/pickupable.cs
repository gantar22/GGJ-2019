using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickupable : MonoBehaviour
{
    protected bool firstPickup = true;

    public void OnPickupEvent()
    {
       OnPickup();
       firstPickup = false;
    }

    protected virtual void OnPickup()
    {
        Debug.Log("BBBB");
    }
    
}
