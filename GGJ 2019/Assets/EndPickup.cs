using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPickup : pickupable
{
    [SerializeField]
    int_var creepiness;

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
        shouldOutlineOnHover = false;
        Application.Quit();
    }
}
