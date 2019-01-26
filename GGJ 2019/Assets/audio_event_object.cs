using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public struct AudioEvent
{
    public Vector3 location;
    public AudioClip clip;
}

[CreateAssetMenu(menuName = "events/audio_event")]
public class audio_event_object : ScriptableObject
{
    [SerializeField]
    AudioEvent constant;

    public class string_event : UnityEvent<AudioEvent> { }

    public UnityEvent<AudioEvent> e = new string_event();

    public void Invoke(string d) { e.Invoke(d); }

    public void Invoke() { e.Invoke(constant); }

}
