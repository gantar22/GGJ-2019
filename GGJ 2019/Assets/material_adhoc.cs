using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class material_adhoc : MonoBehaviour
{

    [SerializeField]
    Vector2 tiling;
    [SerializeField]
    public Vector2 offset;

    [SerializeField]
    [Range(0, 1)]
    public float magnitude;


    Material m;

    // Start is called before the first frame update
    void Start()
    {
        m = GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        m.mainTextureScale = tiling;
        m.mainTextureOffset = offset;
        m.SetFloat("_Magnitude", magnitude);
    }
}
