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
    bool_var dialogue_running;

    [SerializeField]
    float rotate_speed;

    [SerializeField]
    PostProcessVolume blur_volume;

    [SerializeField]
    float grab_distance;

    Vector3 vel;

    [SerializeField]
    float smoothing;

    private void Start () {
        cam = Camera.main;
    }
	
	private void Update () {
        RaycastHit hit1, hit2;
        int clickable_layer = (1 << LayerMask.NameToLayer("interactable")) | (1 << LayerMask.NameToLayer("pickupable"));
        bool hover_pickupable   = Physics.Raycast(cam.transform.position, cam.transform.forward, out hit1, grab_distance,clickable_layer);
        bool hover_interactable = Physics.Raycast(cam.transform.position, cam.transform.forward, out hit2, grab_distance, clickable_layer);

        if(hit1.collider)
            hover_pickupable = hit1.collider.gameObject.layer == LayerMask.NameToLayer("pickupable");
        if(hit2.collider)
            hover_interactable = hit2.collider.gameObject.layer == LayerMask.NameToLayer("interactable");
        if (hit1.collider) print(hit1.collider.gameObject);

        bool click = Input.GetMouseButtonDown(0) && stun_lock == 0;
        if (hover_pickupable)
        {
            pickupable p = hit1.collider.GetComponent<pickupable>();
            if (p)
            { 
                p.OnHover();
            }
            if (click)
            {
                if (p)
                    p.OnPickupEvent();
                StartCoroutine(go(Camera.main.transform.forward * 1.6f + Camera.main.transform.position, hit1.collider.gameObject));
            }
        }

        if (hover_interactable)
        {
            Interactable p = hit1.collider.GetComponent<Interactable>();
            if (p)
            {
                p.OnHover();
            }
            if(click) hit2.collider.gameObject.GetComponentInParent<Interactable>().act();
        }
	}


    IEnumerator go(Vector3 target,GameObject o)
    {
        Vector3 start = o.transform.position;
        Vector3 cur = o.transform.position;
        Quaternion init_rot = o.transform.rotation;
        //float total_height = target.y - o.transform.position.y;
        vel = Vector3.zero;
        stun_lock.val++;
        o.layer = LayerMask.NameToLayer("picked_up");
        foreach(Transform t in o.transform)
        {
            t.gameObject.layer = LayerMask.NameToLayer("picked_up");
        }
        StartCoroutine(rotaty_rotaty(o));
        float dist = Vector3.Distance(o.transform.position, target);
        while ( Vector3.Distance(o.transform.position,target) > .25f)
        {
            blur_volume.weight = Mathf.Lerp(0,1,1 - Vector3.Distance(o.transform.position, target) / dist);
            cur = Vector3.SmoothDamp(cur, target, ref vel, smoothing);
            o.transform.position = cur;//new Vector3(cur.x, total_height * Mathf.Pow((cur.y - start.y) / total_height, 3) + start.y, cur.z);
            yield return null;
        }
        
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Mouse0) && !dialogue_running.val);

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
        foreach (Transform t in o.transform)
        {
            t.gameObject.layer = LayerMask.NameToLayer("pickupable");
        }
    }


    IEnumerator rotaty_rotaty(GameObject o)
    {
        while(stun_lock > 0)
        {
            if (o.GetComponent<walkman>())
            {
                o.GetComponent<walkman>().tune(Input.GetAxis("Mouse X") * Time.deltaTime);
                o.transform.rotation = Quaternion.Lerp(o.transform.rotation, transform.rotation * Quaternion.Euler(0, -90, -35), Time.deltaTime * 3);
            }
            else
                o.transform.Rotate(new Vector3(0, Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X")) * Time.deltaTime * rotate_speed);
            yield return null;
        }

    }
}
