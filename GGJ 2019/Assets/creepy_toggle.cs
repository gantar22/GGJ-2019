using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class creepy_toggle : MonoBehaviour
{

    [SerializeField]
    int_var creep;


    [SerializeField]
    GameObject_Int to_enable;

    [SerializeField]
    GameObject_Int to_disable;

    // Update is called once per frame
    void Start()
    {
        creep.val = 0;
        foreach(int i in to_enable.Values)
            StartCoroutine(go(i,to_enable.Where(P => P.Value == i).Select(P => P.Key).ToList()));
        foreach (int i in to_disable.Values)
            StartCoroutine(gu(i, to_disable.Where(P => P.Value == i).Select(P => P.Key).ToList()));
    }

    IEnumerator go(int threshold,List<GameObject> L)
    {
        yield return new WaitUntil(() => creep >= threshold);
        foreach(GameObject gobj in L) gobj.SetActive(true);
    }

    IEnumerator gu(int threshold, List<GameObject> L)
    {
        yield return new WaitUntil(() => creep >= threshold);
        foreach (GameObject gobj in L) gobj.SetActive(false);
    }
}

