using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    #region Constant Variables
    private readonly static int maxSailAngle = 45;
    private readonly static int minSpeed = 5;
    private readonly static float sailRotation = 0.4f;
    private readonly static float steerRotation = 0.01f;
    #endregion

    [Header("Sailing")]
    [Range(0,25)] public int thurst;

    public Transform frontSail, midSail, backSail;

    float sailAngle;
    Rigidbody rB;


    [Header("Debug")]
    public bool debug;

    void Start()
    {
        rB = GetComponent<Rigidbody>();
        sailAngle = 0;

        Cursor.visible = false;
    }

    private void Update()
    {
        
    }

    void FixedUpdate()
    {
        Sail();
        Steer();
    }

    private void Steer()
    {
        float steerHorz = Input.GetAxis("Horizontal");

        if (steerHorz != 0)
            rB.AddTorque(new Vector3(0, steerHorz * steerRotation, 0), ForceMode.VelocityChange);

        float speed = GetSailSpeed();
        rB.velocity = new Vector3(transform.forward.x * speed, 0, transform.forward.z * speed);
    }

    private void Sail()
    {
        if (Input.GetButton("Sailing"))
        {
            float sailsHorz = Input.GetAxis("Sailing");

            sailAngle += sailsHorz * sailRotation;

            sailAngle = Mathf.Clamp(sailAngle, -maxSailAngle, maxSailAngle);

            frontSail.localRotation = Quaternion.AngleAxis(sailAngle, Vector3.up);
            midSail.localRotation = Quaternion.AngleAxis(sailAngle, Vector3.up);
            backSail.localRotation = Quaternion.AngleAxis(sailAngle, Vector3.up);
        }
    }

    private void OnGUI()
    {
        if (debug)
        {
            GUI.Label(new Rect(0, 0, 100, 25), "Ship Data");
            GUI.Label(new Rect(0, 25, 300, 25), "Sail Speed: " + GetSailSpeed());
            GUI.Label(new Rect(0, 50, 300, 25), "Velocity: " + rB.velocity.ToString());
        }
    }

    private void OnDrawGizmos()
    {
        if (debug)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(new Vector3(transform.position.x, transform.position.y + 25, transform.position.z), frontSail.forward * 10);

            Gizmos.color = Color.blue;
            Gizmos.DrawRay(new Vector3(transform.position.x, transform.position.y + 25, transform.position.z), WindManager.instance.getDirection() * 10);
        }
    }

    float GetSailSpeed()
    {
        return (Mathf.Clamp01(Vector3.Dot(frontSail.forward, WindManager.instance.getDirection())) * (thurst - minSpeed)) + minSpeed;
    }
}
