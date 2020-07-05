using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    public Material wave;

    PlaneGenerator p;
    Mesh m;
   
    void Start()
    {
        // Generate Plane
        p = GetComponent<PlaneGenerator>();
        p.Generate();

        // Attach Wave Material & Remove Shadow Casting
        MeshRenderer mR = GetComponent<MeshRenderer>();
        mR.material = wave;
        mR.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                
        m = GetComponent<MeshFilter>().mesh;
    }

}
