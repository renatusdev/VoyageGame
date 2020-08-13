using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Wave : MonoBehaviour
{
    public Transform ship;

    void Start()
    {
        // Remove Shadow Casting
        MeshRenderer mR = GetComponent<MeshRenderer>();
        mR.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
    }

    private void LateUpdate()
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        transform.position = new Vector3(ship.position.x, transform.position.y, ship.position.z);
    }
}
