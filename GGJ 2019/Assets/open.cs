using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public virtual void act()
    {

    }
}

public class open : Interactable
{

    [SerializeField]
    float rot_speed = 5;

    public override void act()
    {
        GetComponent<Animator>().SetTrigger("open");
    }


}
