using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class adhoc_trigger : MonoBehaviour
{


    [SerializeField]
    event_object t;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(go());
    }

    IEnumerator go()
    {
        yield return new WaitUntil(() => GetComponent<Renderer>().enabled == false);
        t.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
