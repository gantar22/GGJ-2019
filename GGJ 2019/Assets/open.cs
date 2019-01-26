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
        StartCoroutine(go());
    }

    IEnumerator go()
    {
        float theta = 0;
        while(theta > -Mathf.PI / 2)
        {
            transform.rotation = Quaternion.Euler(Vector3.up * theta);
            theta -= Time.deltaTime * rot_speed;
            yield return null;
        }
    }
}
