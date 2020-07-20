using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Should be stored under a parent class called "Floaters".
 * 
 */ 
public class Floater : MonoBehaviour
{

    public float waterVelocityDrag = 0.99f;
    public float waterAngularDrag = 0.5f;

    [Range(1, 5)] public float strength;
    [Range(1, 5)] public float objectDepth;

    #region Private Fields
    int floaters;
    Rigidbody rB;
    #endregion

    private void Start()
    {
        rB = GetComponentInParent<Rigidbody>();
        floaters = transform.parent.childCount;

        if (rB.useGravity)
            rB.useGravity = false;
    }

    // There's a bouncyness to these forces, if submerged = 1 then it bounces up. We want a smooth upwards transition.

    private void FixedUpdate()
    {
        float y = transform.position.y;
        float wH = WaveManager.instance.getHeight(transform.position.x, transform.position.z);

        // Manual gravity subdivided based on the amount of floaters.
        rB.AddForce(Physics.gravity / floaters);

        // If the floater is below water
        if(y <= wH)
        {
            float submersion = Mathf.Clamp01(wH - y) / objectDepth;
            float buoyancy = Mathf.Abs(Physics.gravity.y) * submersion * strength;

            // Buoyant Force
            rB.AddForceAtPosition(Vector3.up * buoyancy, transform.position, ForceMode.Acceleration);

            // Drag Force
            rB.AddForce(-rB.velocity * waterVelocityDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);

            // Torque Force
            rB.AddTorque(-rB.angularVelocity * waterAngularDrag * Time.fixedDeltaTime, ForceMode.Impulse);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, 0.2f);
    }
}
