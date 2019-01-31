using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

public class Interactable : MonoBehaviour
{
    [SerializeField] private AudioClip interactSound;
    [SerializeField] public bool shouldOutlineOnHover = true;
    [SerializeField] private Outline outline;
    protected bool firstInteract = true;

    public void OnInteractEvent()
    {
       if(interactSound) AudioSource.PlayClipAtPoint(interactSound, transform.position);
       act();
       firstInteract = false;
        
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

    private void LateUpdate()
    {
        if (!hovered && outline)
            outline.enabled = false;
        hovered = false;
    }

    public virtual void act()
    {

    }

}
