using System;
using UnityEngine;


/*
 * NOTE:
 * 
 * a) Floaters apply gravity manually. This is because the automatic gravity property of rigidbody
 * applies gravity at the center of mass, whilst with floaters we apply a percentage of gravity in each floater position.
 * 
 * b) Use the drag properties of the Rigidbody component to adjust the buoyant force's influence.
 */

[RequireComponent(typeof(Rigidbody))]
public class Floater : MonoBehaviour
{
    public Transform forces;

    [Range(0, 1)]
    [Tooltip("The overall buoyant influence over this object. At zero it sinks, at 10 its really bouncy.")]
    public float magnitude = 1;

    [Range(0, 10)]
    [Tooltip("The viscocity of water that stops the object from moving.")]
    public float drag = 0;

    [Range(0, 25)]
    [Tooltip("The viscocity of water that stops the object from rotating.")]
    public float angularDrag = 0; 


    Rigidbody rB;
    Vector3[] points;

    private void Start()
    {
        rB = GetComponent<Rigidbody>();
        rB.useGravity = false;

        points = new Vector3[forces.childCount];
        GenerateForcePositions();    
    }

    void GenerateForcePositions()
    {
        for (int i = 0; i < forces.childCount; i++)
            points[i] = forces.GetChild(i).position;
    }

    void FixedUpdate()
    {
        GenerateForcePositions();

        foreach (Vector3 pos in points)
        {
            float y = pos.y;
            float wH = WaveManager.singleton.getHeight(pos.x, pos.z);

            rB.AddForceAtPosition(Physics.gravity / points.Length, pos, ForceMode.Acceleration);

            if (y < wH)
            {
                float submersion = Mathf.Clamp01(wH - y) * magnitude;
                Vector3 buoyancy = new Vector3(0, Math.Abs(Physics.gravity.y) * submersion, 0);

                rB.AddForceAtPosition(buoyancy, pos, ForceMode.Acceleration);
                rB.AddForceAtPosition(-rB.velocity * submersion * drag, pos, ForceMode.Acceleration);
                rB.AddTorque(-rB.angularVelocity * submersion * angularDrag, ForceMode.Acceleration);
            }
        }
    }

    private void OnDrawGizmos()
    {
        points = new Vector3[forces.childCount];
        GenerateForcePositions();
        foreach (Vector3 pos in points)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(pos, 0.3f);
        }

        if (Application.isPlaying)
        {
            float waveHeight = WaveManager.singleton.getHeight(transform.position.x, transform.position.z);

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(new Vector3(transform.position.x, waveHeight, transform.position.z), 0.5f);
        }
    }
}