using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

public class pickupable : MonoBehaviour
{
    [SerializeField] private DialogDisplay dialogDisplay;
    [SerializeField] private DialogAsset dialog;
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private AudioClip letGoSound;
    protected bool firstPickup = true;

    [SerializeField] private bool shouldOutlineOnHover = true;

    private Outline outline;

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

    private bool hovered = false; // hack, i'm sorry
    public void OnHover()
    {
        hovered = true;
        if (shouldOutlineOnHover)
        {
            if (!outline)
                outline = gameObject.AddComponent<Outline>();
            outline.enabled = true;
        }
    }

    protected virtual void OnPickup()
    {

    }

    protected virtual void OnLetGo()
    {

    }

    private void LateUpdate()
    {
        if (!hovered && outline)
            outline.enabled = false;
        hovered = false;
    }

}
