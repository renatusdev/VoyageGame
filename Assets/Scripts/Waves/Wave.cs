using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * 
 * 
 * Wave Analysis:
 * 
 * - Direction Organization.
 *      Wave A: Determines overall direction
 *      Wave B & C: Axis of direction must have same sign (min value +-0.3).
 *      
 *      Waves Direction Example (South)
 *      WaveA.dir = (0,-1)
 *      WaveB.dir = (0.2, -0.7)
 *      WaveC.dir = (-0.3, -0.3)
 */
public class Wave : MonoBehaviour
{
    public Transform ship;

    public Vector3 currWind;
    
    private Material waveMat;

    void Start()
    {
        // Remove Shadow Casting
        MeshRenderer mR = GetComponent<MeshRenderer>();
        mR.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        waveMat = mR.material;
    }

    // Should get wind data and apply it.


    private void LateUpdate()
    {
        UpdatePosition();
        UpdateWind();
    }

    private void UpdatePosition()
    {
        transform.position = new Vector3(ship.position.x, transform.position.y, ship.position.z);
    }

    // Eventually should have lerping
    private void UpdateWind()
    {
        if (currWind != WindManager.instance.getDirection())
        {
            currWind = WindManager.instance.getDirection();

            Vector4 wave = waveMat.GetVector("_WaveA");
            waveMat.SetVector("_WaveA", new Vector4(currWind.x, currWind.z, wave.z, wave.w));

            wave = waveMat.GetVector("_WaveB");
            waveMat.SetVector("_WaveB", new Vector4(Mathf.Sign(currWind.x) * UnityEngine.Random.Range(0.3f, 0.7f), Mathf.Sign(currWind.z) * UnityEngine.Random.Range(0.3f,0.7f), wave.z, wave.w));

            wave = waveMat.GetVector("_WaveC");
            waveMat.SetVector("_WaveB", new Vector4(UnityEngine.Random.Range(-0.7f, 0.7f), UnityEngine.Random.Range(-0.7f, 0.7f), wave.z, wave.w));

            WaveManager.instance.UpdateWaveData();
        }
    }
}
