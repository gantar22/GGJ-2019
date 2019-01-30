using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shelf : Interactable
{

    [SerializeField]
    Vector3 offset;
    private Vector3 startPoint;
    [SerializeField]
    private bool opened = false;

    [SerializeField] private AudioClip openSound;
    [SerializeField] private AudioClip closeSound;

    private void Start()
    {
        startPoint = transform.position;
    }

    public override void act()
    {
        opened = !opened;
        if (opened)
        {
             if(openSound) AudioSource.PlayClipAtPoint(openSound, transform.position);
        }
        else
        {
             if(closeSound) AudioSource.PlayClipAtPoint(closeSound, transform.position);
        }
    }

    private void Update()
    {
        if(opened)
        {
            transform.position = Vector3.Lerp(transform.position, startPoint + offset, 0.1f);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, startPoint, 0.1f);
        }
    }


}

