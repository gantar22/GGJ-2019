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
public class audio_event_object : gen_event<Vector3,AudioClip>{}
