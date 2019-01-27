using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class atmospheric_music_player : MonoBehaviour
{

    [SerializeField] private AudioSource source_fade_in;
    [SerializeField] private AudioSource source_fade_out;
    [SerializeField] private AudioSource source;
    [SerializeField] int_var creepiness;
    [SerializeField] AudioClip[] creepiness_athmo;
    [SerializeField] float fade_time = 5f;
    private float fade_time_current;

    AudioClip nextAthmo;
    int currentCreepinessUsed;

    // Start is called before the first frame update
    void Start()
    {
        nextAthmo = null;
        currentCreepinessUsed = -1;
        fade_time_current = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(creepiness.val > currentCreepinessUsed)
        {
            currentCreepinessUsed = creepiness.val;
            StartNextAthmo();
        }

        if(nextAthmo)
        {
            fade_time_current += Time.deltaTime;
            if(fade_time_current >= fade_time)
            {
                source.clip = source_fade_in.clip;
                source.time = source_fade_in.time;
                source.Play();

                source_fade_out.Stop();
                source_fade_in.Stop();

                nextAthmo = null;
            }
            else
            {
                source_fade_in.volume = fade_time_current/fade_time;
                source_fade_out.volume = 1.0f - fade_time_current/fade_time;
            }
        }
    }

    void StartNextAthmo()
    {
        fade_time_current = 0;
        nextAthmo = creepiness_athmo[Mathf.Min(creepiness.val,creepiness_athmo.Length-1)];
        if(nextAthmo)
        {
            if(creepiness.val > 0 && creepiness_athmo[Mathf.Min(creepiness.val-1,creepiness_athmo.Length-1)])
            {
                source_fade_out.clip = source.clip;
                source_fade_out.time = source.time;
                source_fade_out.Play();
            }
            source.Stop();
            source_fade_in.clip = nextAthmo;
            source_fade_in.Play();
        }
    }


}
