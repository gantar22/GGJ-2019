using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class adhoc_tv_script : MonoBehaviour
{
    [SerializeField]
    event_object trigger;

    // Start is called before the first frame update
    void Start()
    {
        trigger.addListener(go);
    }


    void go()
    {
        GetComponent<Animator>().SetTrigger("go");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
