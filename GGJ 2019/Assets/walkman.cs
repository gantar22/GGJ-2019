using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class walkman : pickupable
{

    [SerializeField]
    Transform slider;
    [SerializeField]
    Transform slider_left;
    [SerializeField]
    Transform slider_right;
    public float volume;
    [SerializeField]
    float rotate_speed = 5;
    [SerializeField]
    bool_var dialog;
    [SerializeField]
    AudioSource ass;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void OnPickup()
    {
       
    }

    IEnumerator play_sound()
    {
        dialog.val = true;
        ass.enabled = true;
        yield return null;


    }

    public void tune(float t)
    {
        volume += t * rotate_speed;
        volume = Mathf.Clamp(0, 1, volume);
        slider.position = Vector3.Lerp(slider_left.position,slider_right.position,volume);
    }
}
