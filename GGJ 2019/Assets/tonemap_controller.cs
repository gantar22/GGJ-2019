using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class tonemap_controller : MonoBehaviour
{

    PostProcessVolume postpro_in;
    PostProcessVolume postpro_out;

    [SerializeField]
    int_var creepiness;
    [SerializeField]
    PostProcessVolume[] creepiness_postpro;
    [SerializeField]
    float fade_time = 5f;
    private float fade_time_current;

    int currentCreepinessUsed;

    // Start is called before the first frame update
    void Start()
    {
        currentCreepinessUsed = 0;
        fade_time_current = 0;
        creepiness_postpro[0].weight = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (creepiness.val > currentCreepinessUsed)
        {
            currentCreepinessUsed = creepiness.val;
            StartNextAthmo();
        }

        fade_time_current += Time.deltaTime;
        if (fade_time_current >= fade_time)
        {
            fade_time_current = fade_time;
        }
        postpro_in.weight = fade_time_current / fade_time;
        postpro_out.weight = 1.0f - fade_time_current / fade_time;
    }

    void StartNextAthmo()
    {
        fade_time_current = 0;
        postpro_in = creepiness_postpro[Mathf.Min(creepiness.val, creepiness_postpro.Length - 1)];
        postpro_out = creepiness_postpro[Mathf.Min(creepiness.val-1, creepiness_postpro.Length - 2)];
        postpro_in.weight = 0;
        postpro_out.weight = 1;
    }
}
