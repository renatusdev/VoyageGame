using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CannonController : MonoBehaviour
{
    private readonly static int minHorzAngle = 50;
    private readonly static int maxHorzAngle = 125;

    private readonly static int minVertAngle = 8;
    private readonly static int maxVertAngle = 25;
    private readonly static int vertOffset = 25;

    [Header("Cannons")]
    public Transform leftC;
    public Transform rightC;

    public CannonType current;
    public Material mat;

    public CinemachineVirtualCamera cm;

    private CinemachinePOV pov;

    private void Start()
    {
        pov = cm.GetCinemachineComponent<CinemachinePOV>();
    }

    private void Update()
    {
        float horzMouse = pov.m_HorizontalAxis.Value;
        float vertMouse = pov.m_VerticalAxis.Value;

        int cannonSide = (int)Mathf.Sign(horzMouse);

        Aim(transform.GetChild((cannonSide + 1) / 2), Mathf.Abs(horzMouse), vertMouse, cannonSide);
    }

    private void Aim(Transform c, float horzMouse, float vertMouse, int cannonSide)
    {
        // Y-axis Rotation (Left & Right)
        Quaternion q = Quaternion.AngleAxis(Mathf.Clamp(horzMouse, minHorzAngle, maxHorzAngle) * cannonSide, Vector3.up);

        // X-axis Rotation (Up & Down)
        q *= Quaternion.AngleAxis(Mathf.Clamp(vertMouse, minVertAngle, maxVertAngle) - vertOffset, Vector3.right);

        c.localRotation = q;

        if (Input.GetButtonDown("Fire1"))
            Fire(c, cannonSide);
    }

    private void Fire(Transform c, int cannonSide)
    {
        CinemachineShake.instance.Shake(5, 0.4f);

        transform.GetChild((cannonSide + 1) / 2 + 2).GetComponent<ParticleSystem>().Play();
        
        foreach (ParticleSystem pS in c.GetComponentsInChildren<ParticleSystem>())
            pS.Play();

        Inventory.instance.cannons[current].Fire();
    }
}