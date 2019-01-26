using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOutColor : MonoBehaviour
{
    [SerializeField] float beforeFadeOutTime = 3f;
    [SerializeField] float fadeOutTime = 1f;
    private float currentFadeOutTime = 0.0f;
    

    // Start is called before the first frame update
    void Start()
    {
        currentFadeOutTime = fadeOutTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(beforeFadeOutTime <= 0)
        {
            if (currentFadeOutTime > 0)
            {
                currentFadeOutTime -= Time.deltaTime;
            }
            else
            {
                currentFadeOutTime = 0;
            }

            Graphic[] images = GetComponents<Graphic>();
            foreach (Graphic i in images)
            {
                i.color = new Color(i.color.r, i.color.g, i.color.b, currentFadeOutTime / fadeOutTime);
            }
        }
        else
        {
            beforeFadeOutTime -= Time.deltaTime;
        }

    }
}
