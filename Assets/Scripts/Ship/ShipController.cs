using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public enum ShipState { Sailing, Ramming }
public class ShipController : MonoBehaviour
{
    #region Constant Variables
    private readonly static int maxSailAngle = 45;
    private readonly static int minSpeed = 5;
    private readonly static float sailRotation = 0.4f;
    private readonly static float steerRotation = 0.1f;
    private readonly static float ramFOV = 50;
    private readonly static float ramShake = 2;
    private readonly static float ramLenseDistor = 0.4f;
    private readonly static float ramRecovery = 1.7f;
    #endregion

    public static float FOV = 75;
    public static int ramSpeed = 50;

    [Header("Sailing")]
    [Range(0,45)] public int thurst;

    public Transform frontSail, midSail, backSail;
    public CinemachineVirtualCamera cam;

    ShipState currState;
    LensDistortion lD;
    float sailAngle, ram;
    Rigidbody rB;

    [Header("Debug")]
    public bool debug;

    void Start()
    {
        rB = GetComponent<Rigidbody>();
        Camera.main.GetComponent<Volume>().profile.TryGet(out lD); 

        if (rB.useGravity)
            rB.useGravity = false;
        sailAngle = 0;
        cam.m_Lens.FieldOfView = FOV;
        ChangeState(ShipState.Sailing);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (currState.Equals(ShipState.Ramming))
        {
            ChangeState(ShipState.Sailing);
        }
    }

    void FixedUpdate()
    {
        Ram();
        Sail();
        Steer();
    }

    private void Ram()
    {   
        if(Input.GetKeyDown(KeyCode.W))
            ChangeState(ShipState.Ramming);

        if (Input.GetKey(KeyCode.W) & currState.Equals(ShipState.Ramming))
        {
            if (!CinemachineShake.instance.IsActive())
                CinemachineShake.instance.Shake(ramShake, 1);
            
            ram = Mathf.Lerp(ram, ramSpeed, Time.deltaTime);
            cam.m_Lens.FieldOfView = Mathf.Lerp(cam.m_Lens.FieldOfView, ramFOV, Time.deltaTime);
            lD.intensity.value = Mathf.Lerp(lD.intensity.value, -ramLenseDistor, Time.deltaTime);
        }
        else
        {
            ram = Mathf.Lerp(ram, 0, Time.deltaTime * ramRecovery);
            cam.m_Lens.FieldOfView = Mathf.Lerp(cam.m_Lens.FieldOfView, FOV, Time.deltaTime * ramRecovery);
            lD.intensity.value = Mathf.Lerp(lD.intensity.value, 0, Time.deltaTime * ramRecovery);
        }
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
        if (debug & Application.isPlaying)
        {
            

            GUI.Label(new Rect(0, 0, 100, 25), "Ship Data");
            GUI.Label(new Rect(0, 25, 300, 25), "Sail Speed: " + GetSailSpeed());
            GUI.Label(new Rect(0, 50, 300, 25), "Velocity: " + rB.velocity.ToString());
        }
    }

    private void OnDrawGizmos()
    {
        if (debug & Application.isPlaying)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(new Vector3(transform.position.x, transform.position.y + 25, transform.position.z), frontSail.forward * 10);

            Gizmos.color = Color.blue;
            Gizmos.DrawRay(new Vector3(transform.position.x, transform.position.y + 25, transform.position.z), WindManager.instance.getDirection() * 10);
        }
    }

    float GetSailSpeed()
    {
        return (Mathf.Clamp01(Vector3.Dot(frontSail.forward, WindManager.instance.getDirection())) * (thurst - minSpeed)) + minSpeed + ram;
    }


    public void ChangeState(ShipState state) { currState = state; }
}
