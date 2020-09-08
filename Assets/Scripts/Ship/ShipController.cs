using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ShipController : Ship
{
    #region Constant Variables
    private readonly static int gravity = 10;

    private readonly static int maxSailAngle = 45;
    private readonly static float sailRotation = 0.4f;
    
    private readonly static int minSpeed = 25;
    private readonly static float steerRotation = 0.1f;

    private readonly static float ramFOV = 50;              // Ram property for FOV
    private readonly static float ramShake = 2;             // Ram property for Camera Shake
    private readonly static float ramLenseDistor = 0.4f;    // Ram property for Lense Distortion
    private readonly static float ramGPXRecovery = 1.7f;    // Time modifier for visual effects to return to normal after ramming
    private readonly static float ramRecovery = 5;          // Time modifier for ram cooldown
    private readonly static int ramMaxEnergy = 4;           // Time modifier for ram effect
    #endregion

    #region Public Variables
    [Header("Sailing")] 
    public Transform frontSail, midSail, backSail;

    [Header("Prefabs Used")]
    public CinemachineVirtualCamera cam;
    #endregion

    #region Customizable Variables
    public static float FOV = 100;
    private static float ramEnergy;
    public static int ramMaxSpeed = 50;
    #endregion

    #region Other Variables
    private static float sailAngle;
    private static float ramSpeed;
    private static bool ramCooling;
    private static LensDistortion lD;
    private static CinemachineFramingTransposer body;
    #endregion

    #region Debug Variables
    [Header("Debug")]
    public bool debug;
    #endregion

    void Start()
    {
        Camera.main.GetComponent<Volume>().profile.TryGet(out lD);
        body = cam.GetCinemachineComponent<CinemachineFramingTransposer>();
        // rb = GetComponent<Rigidbody>();

        // if (rb.useGravity)
        //     rb.useGravity = false;

        cam.m_Lens.FieldOfView = FOV;
        ramEnergy = ramMaxEnergy;
        ramCooling = false;
        sailAngle = 0;
    }

    private void Update()
    {
        RamShip();
        TrimSails();
        body.m_CameraDistance += Input.mouseScrollDelta.y * 3;

        // TEMP
        if (Input.GetKeyDown(KeyCode.M))
            CreatureEvent.PlayEvent(Creature.Fishes);
    }

    void FixedUpdate()
    {
        SteerHelm();
    }

    protected override void RamShip()
    {
        float t = Time.deltaTime;
        if (Input.GetKey(KeyCode.W) & !ramCooling)
        {
            // Ramming logic
            ramEnergy -= t;
            ramCooling = (ramEnergy <= 0);
            ramSpeed = Mathf.Lerp(ramSpeed, ramMaxSpeed, t);

            // Effects
            cam.m_Lens.FieldOfView = Mathf.Lerp(cam.m_Lens.FieldOfView, ramFOV, t);
            lD.intensity.value = Mathf.Lerp(lD.intensity.value, -ramLenseDistor, t);
        }   
        else
        {
            // Ramming logic
            ramSpeed = Mathf.Lerp(ramSpeed, 0, Time.deltaTime * ramGPXRecovery);
            ramEnergy = Mathf.Clamp(ramEnergy + (t/ramRecovery), 0, ramMaxEnergy);
            ramCooling = (ramEnergy < ramMaxEnergy);

            // Effects
            cam.m_Lens.FieldOfView = Mathf.Lerp(cam.m_Lens.FieldOfView, FOV, t * ramGPXRecovery);
            lD.intensity.value = Mathf.Lerp(lD.intensity.value, 0, t * ramGPXRecovery);
        }
    }   

    protected override void SteerHelm() 
    {
        float steerHorz = Input.GetAxis("Horizontal");

            if (steerHorz != 0)
                rb.AddTorque(new Vector3(0, steerHorz * steerRotation, 0), ForceMode.VelocityChange);

        float speed = GetSailSpeed();
        rb.velocity = new Vector3(transform.forward.x * speed, -gravity, transform.forward.z * speed);
    }   

    protected override void TrimSails()
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
            GUI.Label(new Rect(0, 50, 300, 25), "Velocity: " + rb.velocity.ToString());
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
        return (Mathf.Clamp01(Vector3.Dot(frontSail.forward, WindManager.instance.GetDirection())) * (speed - minSpeed)) + minSpeed + ramSpeed;
    }
}
