using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;


/*
* This script listens for a mouse down
* then assigns any object in the pickupable
* layer to the to_pick_up GO variable 
*/
public class pick_up : MonoBehaviour {

    Camera cam;

    [SerializeField]
    int_var stun_lock;

    [SerializeField]
    float rotate_speed;

    [SerializeField]
    PostProcessVolume blur_volume;
    

    Vector3 vel;

    [SerializeField]
    float smoothing;


    // Use this for initialization
    void Start () {
        cam = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(1) && stun_lock == 0)
        {
            RaycastHit hit = new RaycastHit();
            bool b = Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("pickupable"));

            if (b)
            {
                //to_pick_up.val = hit.collider.gameObject;
                pickupable p = hit.collider.GetComponent<pickupable>();
                if(p)
                {
                    p.OnPickupEvent();
                }
                StartCoroutine(go(Camera.main.transform.forward * 2 + Camera.main.transform.position, hit.collider.gameObject));
            }
            b = Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("interactable"));

            if (b)
            {
                hit.collider.gameObject.GetComponent<Interactable>().act();
            }
        }
	}


    IEnumerator go(Vector3 target,GameObject o)
    {
        Vector3 start = o.transform.position;
        Vector3 cur = o.transform.position;
        //float total_height = target.y - o.transform.position.y;
        vel = Vector3.zero;
        stun_lock.val++;
        o.layer = LayerMask.NameToLayer("picked_up");
        StartCoroutine(rotaty_rotaty(o));
        float dist = Vector3.Distance(o.transform.position, target);
        while (Input.GetKey(KeyCode.Mouse1) && Vector3.Distance(o.transform.position,target) > .25f)
        {
            blur_volume.weight = Mathf.Lerp(0,1,1 - Vector3.Distance(o.transform.position, target) / dist);
            cur = Vector3.SmoothDamp(cur, target, ref vel, smoothing);
            o.transform.position = cur;//new Vector3(cur.x, total_height * Mathf.Pow((cur.y - start.y) / total_height, 3) + start.y, cur.z);
            yield return null;
        }
        
        yield return new WaitUntil(() => !Input.GetKey(KeyCode.Mouse1));
        
        Quaternion q = o.transform.rotation;
        dist = Vector3.Distance(o.transform.position, start);
        while (Vector3.Distance(o.transform.position, start) > .3f)
        {
            blur_volume.weight = Mathf.Lerp(1,0, 1 - Vector3.Distance(o.transform.position, start) / dist);
            cur = Vector3.SmoothDamp(cur, start, ref vel, smoothing / 2f);
            o.transform.position = cur;//new Vector3(cur.x, total_height * Mathf.Pow((cur.y - start.y) / total_height, 3) + start.y, cur.z);
            o.transform.rotation = Quaternion.Lerp(q, Quaternion.identity,1 - Vector3.Distance(o.transform.position, start) / dist);
            yield return null;
        }
        blur_volume.weight = 0;
        o.transform.position = start;
        o.transform.rotation = Quaternion.identity;
        stun_lock.val--;
        o.layer = LayerMask.NameToLayer("pickupable");
    }


    IEnumerator rotaty_rotaty(GameObject o)
    {
        while(Input.GetKey(KeyCode.Mouse1))
        {
            o.transform.Rotate(new Vector3(Input.GetAxis("Mouse Y"),-Input.GetAxis("Mouse X"),0) * Time.deltaTime * rotate_speed);
            yield return null;
        }

    }
}
