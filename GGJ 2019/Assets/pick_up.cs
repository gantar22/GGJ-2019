using System.Collections;
using System.Collections.Generic;
using UnityEngine;




/*
* This script listens for a mouse down
* then assigns any object in the pickupable
* layer to the to_pick_up GO variable 
*/
public class pick_up : MonoBehaviour {

    Camera cam;

    [SerializeField]
    go_var to_pick_up;


	// Use this for initialization
	void Start () {
        cam = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetMouseButtonDown(1))
        {
            RaycastHit hit = new RaycastHit();
            bool b = Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("pickupable"));

            if(b)
            {
                to_pick_up.val = hit.collider.gameObject;
            }
        }
	}
}
