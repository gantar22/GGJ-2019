using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class water_animator : MonoBehaviour
{

    [SerializeField]
    float lead_time;

    [SerializeField]
    float amplitude;

    [SerializeField]
    float freq;

    [SerializeField]
    float max_mag;

    [SerializeField]
    material_adhoc ma;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(go());
    }

    IEnumerator go()
    {
        float t = 0f;

        while(t < lead_time)
        {
            t += Time.deltaTime;
            ma.magnitude = t * max_mag / lead_time;

            yield return null;
        }

        while(true)
        {

            yield return null;
        }
    }
}
