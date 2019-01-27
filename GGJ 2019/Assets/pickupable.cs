using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickupable : MonoBehaviour
{
    [SerializeField] private DialogDisplay dialogDisplay;
    [SerializeField] private DialogAsset dialog;
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private AudioClip letGoSound;
    protected bool firstPickup = true;

    public void OnPickupEvent()
    {
        if( dialogDisplay && dialog)
        {
            dialogDisplay.SetDialog(dialog);
            dialogDisplay.Display();
        }
        if(pickupSound) AudioSource.PlayClipAtPoint(pickupSound, transform.position);
       OnPickup();
       firstPickup = false;
    }

    public void OnLetGoEvent()
    {
        if (letGoSound) AudioSource.PlayClipAtPoint(letGoSound, transform.position);
        OnLetGo();
    }

    protected virtual void OnPickup()
    {

    }

    protected virtual void OnLetGo()
    {

    }

}
