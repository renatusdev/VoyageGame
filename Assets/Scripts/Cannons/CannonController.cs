using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    [Header("Cannons")]

    public Transform leftC;
    public Transform rightC;

    public CannonType current;
    public Material mat;

    private Dictionary<CannonType, Cannon> cannons;


    private void Start()
    {
        cannons = new Dictionary<CannonType, Cannon>();
        cannons.Add(CannonType.Normal, new NormalCannon());
        cannons.Add(CannonType.Anchor, new AnchorCannon());

        // For Testing
        cannons[CannonType.Normal].AddAmount(5);
        cannons[CannonType.Anchor].AddAmount(5);
    }

    private void Update()
    {
        // Use cinemachine aim horizontal value as aiming between left and right.

        Fire();
    }

    private void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            // Changing Cannon Sprite ???
            
            foreach (ParticleSystem pS in rightC.GetComponentsInChildren<ParticleSystem>())
            {
                pS.Play();
            }


            cannons[current].Fire();
        }
    }
}

public enum CannonType { Normal, Anchor, Egg }