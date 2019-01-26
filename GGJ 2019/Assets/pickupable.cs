using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickupable : MonoBehaviour {
    Vector3 vel;

    [SerializeField]
    float smoothing;

	IEnumerator go(Vector3 target)
    {
        Vector3 start = transform.position;
        Vector3 cur = Vector3.zero;
        float total_height = target.y - transform.position.y;
        while(true)
        {
            cur = Vector3.SmoothDamp(cur, target, ref vel, smoothing);
            transform.position = new Vector3(cur.x,total_height * Mathf.Pow((cur.y - start.y) / total_height,3),cur.z);
            yield return null;
        }
    }
}
