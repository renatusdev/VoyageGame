using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CannonController : MonoBehaviour
{
    private readonly static int minHAngle = 50;
    private readonly static int maxHAngle = 125;

    // Range is between [-30,30]
    // The lower the mouse position, the higher the fire angle
    // equally, the higher the mouse position, the lower the fire angle
    private readonly static int minVAngle  = -10;
    private readonly static int maxVAngle = 25;

    [Header("Cannons")]
    public Transform leftC;
    public Transform rightC;

    public static CannonType current;
    public Material mat;

    public CinemachineVirtualCamera cm;

    private CinemachinePOV pov;

    private void Start()
    {
        current = CannonType.Normal;
        pov = cm.GetCinemachineComponent<CinemachinePOV>();
    }

    private void Update()
    {
        float h = pov.m_HorizontalAxis.Value;
        float v = pov.m_VerticalAxis.Value;
        int cannonSide = (int)Mathf.Sign(h);

        Aim(transform.GetChild((cannonSide + 1) / 2), Mathf.Abs(h), v, cannonSide);
    }

    private void Aim(Transform c, float h, float v, int cannonSide)
    {
        // Y (Left & Right) and X (Up & Down) -axis rotations 
        Quaternion q = Quaternion.AngleAxis(Mathf.Clamp(h, minHAngle, maxHAngle) * cannonSide, Vector3.up);
        q *= Quaternion.AngleAxis(Mathf.Clamp(v, minVAngle, maxVAngle), Vector3.right);

        c.localRotation = q;

        if (Input.GetButtonDown("Fire1"))
            Fire(c, cannonSide);
    }

    private void Fire(Transform c, int cannonSide)
    {
        SoundManager.PlaySound(Sound.CannonFire);

        transform.GetChild((cannonSide + 1) / 2 + 2).GetComponent<ParticleSystem>().Play();
        
        foreach (ParticleSystem pS in c.GetComponentsInChildren<ParticleSystem>())
            pS.Play();

        Inventory.i.cannons[current].Fire();
    }
}