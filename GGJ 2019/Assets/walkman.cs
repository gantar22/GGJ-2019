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
        StartCoroutine(play_sound());
    }

    IEnumerator play_sound()
    {
        yield return new WaitForSeconds(.1f);
        dialog.val = true;
        ass.enabled = true;
        yield return new WaitUntil(() => !ass.isPlaying);
        dialog.val = false;
        
    }

    public void tune(float t)
    {
        print(t);
        volume += (t * rotate_speed) * Mathf.Pow(1 - volume,3);
        volume = Mathf.Clamp(volume,0, 1);
        ass.volume = volume;
        slider.position = Vector3.Lerp(slider_right.position, slider_left.position,volume);
    }
}
