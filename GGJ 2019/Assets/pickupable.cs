using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickupable : MonoBehaviour
{
    [SerializeField] private AudioClip pickupSound;
    protected bool firstPickup = true;

    public void OnPickupEvent()
    {
       AudioSource.PlayClipAtPoint(pickupSound, transform.position);
       OnPickup();
       firstPickup = false;
    }

    protected virtual void OnPickup()
    {

    }
    
}
