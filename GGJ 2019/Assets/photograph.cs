using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class photograph : pickupable
{
    [SerializeField] int_var creepiness;

    protected override void OnPickup()
    {
        if(firstPickup)
        {
            Debug.Log("AAAA");
            creepiness.val++;
        }
    }
}
