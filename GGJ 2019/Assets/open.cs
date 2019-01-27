using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class open : Interactable
{

    [SerializeField]
    float rot_speed = 5;

    [SerializeField]
    int_event_object obj;

    [SerializeField]
    int index;

    public override void act()
    {
        GetComponent<Animator>().SetTrigger("open");
        if (obj && firstInteract) obj.Invoke(index);
    }


}
