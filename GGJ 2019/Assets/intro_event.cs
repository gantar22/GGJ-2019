using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class intro_event : MonoBehaviour
{
    [SerializeField]
    event_object start;
    [SerializeField]
    int_event_object init_d

;
    [SerializeField]
    event_object end;
    // Start is called before the first frame update
    void Start()
    {
        start.Invoke();
        Invoke("go", 1);
        end.addListener(() =>
        {
            GetComponent<Animator>().SetTrigger("go");
            Invoke("sl",2.5f);
        });
    }

    void sl()
    {
        init_d.Invoke(1);
    }

    void go()
    {
        init_d.Invoke(0);
    }
}
