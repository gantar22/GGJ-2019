using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private AudioClip interactSound;
    protected bool firstInteract = true;

    public void OnInteractEvent()
    {
       if(interactSound) AudioSource.PlayClipAtPoint(interactSound, transform.position);
       act();
       firstInteract = false;
    }

    public virtual void act()
    {

    }

}
