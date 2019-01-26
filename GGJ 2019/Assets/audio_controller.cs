using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audio_controller : MonoBehaviour
{
    [SerializeField] private audio_event_object audio_event;
    [SerializeField] private AudioSource oneShotSource;

    void OnAudioEvent(Vector3 position, AudioClip clip)
    {
        AudioSource.PlayClipAtPoint(clip, position);
    }

    // Start is called before the first frame update
    void Start()
    {
        audio_event.addListener(OnAudioEvent);
    }

}
