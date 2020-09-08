using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp : MonoBehaviour
{
    public Transform target;
    public float dist;

    Rigidbody rb;

    Vector3 bowInfluence;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Temp1()
    {
        Quaternion forw = Quaternion.LookRotation((target.position-transform.position) + bowInfluence);
        Quaternion rot = Quaternion.RotateTowards(transform.rotation, forw, Time.deltaTime * 25);
                            
        rb.MoveRotation(rot);
    }

    void Temp2()
    {
        bowInfluence = Vector3.zero;
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit))
        {
            if(hit.collider.CompareTag("Player"))
            {
                // Rammin'
            }
            else
            {
                // Its an obstacle
                Debug.Log("Dodging");
                bowInfluence -= Vector3.right * 55;
            }
        }
    }

    private void FixedUpdate()
    {
        Temp2();
        Temp1();
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, transform.forward + transform.right * -4);
        Debug.DrawRay(transform.position, transform.forward + transform.right * -1);
        Debug.DrawRay(transform.position, transform.forward + transform.right * -2);
        Debug.DrawRay(transform.position, transform.forward + transform.right * 0);
        Debug.DrawRay(transform.position, transform.forward + transform.right * 1);
        Debug.DrawRay(transform.position, transform.forward + transform.right * 2);
        Debug.DrawRay(transform.position, transform.forward + transform.right * 4);
        
        Vector3 ray = transform.position + (transform.forward + transform.right * -2)*dist;         
        Gizmos.DrawSphere(ray, 0.1f);
    }
}
