using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ShipController : MonoBehaviour
{
    #region Constant Variables
    private readonly static int maxSailAngle = 45;
    private readonly static int minSpeed = 5;
    private readonly static float sailRotation = 0.4f;
    private readonly static float steerRotation = 0.1f;
    private readonly static float ramFOV = 50;              // Ram property for FOV
    private readonly static float ramShake = 2;             // Ram property for Camera Shake
    private readonly static float ramLenseDistor = 0.4f;    // Ram property for Lense Distortion
    private readonly static float ramGPXRecovery = 1.7f;    // Time modifier for visual effects to return to normal after ramming
    private readonly static float ramRecovery = 5;          // Time modifier for ram cooldown
    private readonly static int ramMaxEnergy = 2;           // Time modifier for ram effect
    #endregion

    #region Public Variables
    [Header("Sailing")]
    public Transform frontSail, midSail, backSail;
    public CinemachineVirtualCamera cam;
    [Range(0, 45)] public int thurst;
    #endregion

    #region Static Variables
    public static float FOV = 75;
    public static int ramSpeed = 50;
    public static float ramEnergy;

    private static float sailAngle, ram;
    private static LensDistortion lD;
    private static Rigidbody rB;
    private bool ramCooling;
    #endregion

    #region Debug Variables
    [Header("Debug")]
    public bool debug;
    #endregion

    void Start()
    {
        Camera.main.GetComponent<Volume>().profile.TryGet(out lD);
        rB = GetComponent<Rigidbody>();

        if (rB.useGravity)
            rB.useGravity = false;

        cam.m_Lens.FieldOfView = FOV;
        ramEnergy = ramMaxEnergy;
        ramCooling = false;
        sailAngle = 0;
    }

    private void Update()
    {
        Ram();
        Sail();
        //FOV += Input.mouseScrollDelta.y * 3;
    }

    void FixedUpdate()
    {
        Steer();
    }

    private void Ram()
    {
        if (Input.GetKey(KeyCode.W) & !ramCooling)
        {
            if (!CinemachineShake.instance.IsActive())
                CinemachineShake.instance.Shake(ramShake, 1);

            float t = Time.deltaTime;
            ramEnergy -= t;
            ramCooling = (ramEnergy <= 0);

            ram = Mathf.Lerp(ram, ramSpeed, t);
            cam.m_Lens.FieldOfView = Mathf.Lerp(cam.m_Lens.FieldOfView, ramFOV, t);
            lD.intensity.value = Mathf.Lerp(lD.intensity.value, -ramLenseDistor, t);
        }
        else
        {
            float t = Time.deltaTime;

            ram = Mathf.Lerp(ram, 0, Time.deltaTime * ramGPXRecovery);
            cam.m_Lens.FieldOfView = Mathf.Lerp(cam.m_Lens.FieldOfView, FOV, t * ramGPXRecovery);
            lD.intensity.value = Mathf.Lerp(lD.intensity.value, 0, t * ramGPXRecovery);

            ramEnergy = Mathf.Clamp(ramEnergy + (t/ramRecovery), 0, ramMaxEnergy);
            ramCooling = (ramEnergy < ramMaxEnergy);
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
            GUI.Label(new Rect(0, 75, 300, 25), "Ram Energy: " + ramEnergy);

        }
    }

    private void OnDrawGizmos()
    {
        if (debug & Application.isPlaying)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(new Vector3(transform.position.x, transform.position.y + 25, transform.position.z), frontSail.forward * 10);

            Gizmos.color = Color.blue;
            Gizmos.DrawRay(new Vector3(transform.position.x, transform.position.y + 25, transform.position.z), WindManager.instance.GetDirection() * 10);
        }
    }

    float GetSailSpeed()
    {
        return (Mathf.Clamp01(Vector3.Dot(frontSail.forward, WindManager.instance.GetDirection())) * (thurst - minSpeed)) + minSpeed + ram;
    }
}
