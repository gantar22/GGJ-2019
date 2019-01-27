using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using cakeslice;


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

    private void Start () {
        cam = Camera.main;
    }
	
	private void Update () {
        RaycastHit hit1, hit2;
        bool hover_pickupable   = Physics.Raycast(cam.transform.position, cam.transform.forward, out hit1, Mathf.Infinity, 1 << LayerMask.NameToLayer("pickupable"));
        bool hover_interactable = Physics.Raycast(cam.transform.position, cam.transform.forward, out hit2, Mathf.Infinity, 1 << LayerMask.NameToLayer("interactable"));

        bool click = Input.GetMouseButtonDown(1) && stun_lock == 0;

        if (hover_pickupable)
        {
            pickupable p = hit1.collider.GetComponent<pickupable>();
            p.OnHover();
            if (click)
            {
                if (p)
                    p.OnPickupEvent();
                StartCoroutine(go(Camera.main.transform.forward * 2 + Camera.main.transform.position, hit1.collider.gameObject));
            }
        }

        if(click && hover_interactable)
            hit2.collider.gameObject.GetComponent<Interactable>().act();
	}


    IEnumerator go(Vector3 target,GameObject o)
    {
        Vector3 start = o.transform.position;
        Vector3 cur = o.transform.position;
        Quaternion init_rot = transform.rotation;
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

        pickupable p = o.GetComponent<pickupable>();
        if (p)
        {
            p.OnLetGoEvent();
        }

        Quaternion q = o.transform.rotation;
        dist = Vector3.Distance(o.transform.position, start);
        while (Vector3.Distance(o.transform.position, start) > .3f)
        {
            blur_volume.weight = Mathf.Lerp(1,0, 1 - Vector3.Distance(o.transform.position, start) / dist);
            cur = Vector3.SmoothDamp(cur, start, ref vel, smoothing / 2f);
            o.transform.position = cur;//new Vector3(cur.x, total_height * Mathf.Pow((cur.y - start.y) / total_height, 3) + start.y, cur.z);
            o.transform.rotation = Quaternion.Lerp(q, init_rot,1 - Vector3.Distance(o.transform.position, start) / dist);
            yield return null;
        }
        blur_volume.weight = 0;
        o.transform.position = start;
        o.transform.rotation = init_rot;
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
