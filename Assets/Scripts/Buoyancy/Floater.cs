using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Should be stored under a parent class called "Floaters".
 * 
 */ 
public class Floater : MonoBehaviour
{
    public int floaters;

    public float waterVelocityDrag = 0.99f;
    public float waterAngularDrag = 0.5f;

    [Range(1, 5)] public float strength;
    [Range(1, 5)] public float objectDepth;

    #region Debug Parameters
    public bool debug;
    private bool addingBuoyancy;
    #endregion

    Rigidbody rB;

    private void Start()
    {
        rB = GetComponentInParent<Rigidbody>();
    }

    // There's a bouncyness to these forces, if submerged = 1 then it bounces up. We want a smooth upwards transition.

    private void FixedUpdate()
    {
        float y = transform.position.y;
        float wH = WaveManager.instance.GetHeight(transform.position.x, transform.position.z);

        // Manual gravity subdivided based on the amount of floaters.
        rB.AddForce((Physics.gravity / floaters));

        // If the floater is below water
        if (y <= wH)
        {
            addingBuoyancy = true;

            float submersion = Mathf.Clamp01(wH - y) / objectDepth;
            float buoyancy = Mathf.Abs(Physics.gravity.y) * submersion * strength;

            // Buoyant Force
            rB.AddForceAtPosition(Vector3.up * buoyancy, transform.position, ForceMode.Acceleration);

            // Drag Force
            rB.AddForce(-rB.velocity * waterVelocityDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);

            // Torque Force
            rB.AddTorque(-rB.angularVelocity * waterAngularDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }
        else
            addingBuoyancy = false;
    }

    private void OnDrawGizmos()
    {
        if (debug)
        {
            Gizmos.color = Color.grey;
            Gizmos.DrawSphere(transform.position, 0.2f);

            if (Application.isPlaying & addingBuoyancy)
                Gizmos.color = Color.blue;

            Gizmos.DrawSphere(transform.position, 0.2f);
        }

    }
}
