using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirateCannons : MonoBehaviour
{
    private readonly static int angleClamp = 70;

    Transform cannonSide;

    public void Aim(int side)
    {
        cannonSide = transform.GetChild((side + 1) / 2);
    }

    public void Fire(Vector3 target)
    {
        float spread = UnityEngine.Random.Range(1,1.5f);
        
        // NOTE: if spread is between [0.5 to 1] it will shoot in front of target,
        // and if spread is between [1 to 1.5] it will shoot behidn the target.
        Debug.Log("fire");

        foreach (ParticleSystem pS in cannonSide.GetComponentsInChildren<ParticleSystem>())
        {
            if(CannonCocked(pS.transform))
            {
                pS.transform.LookAt(target);// * UnityEngine.Random.Range(1,1.5f));
                pS.Play();
            }
        }
    }

    public bool CanFire()
    {
        foreach (Transform pS in cannonSide.GetComponentsInChildren<Transform>())
            if(CannonCocked(pS.transform))
                return true;                
        return false;
    }

    private bool CannonCocked(Transform cannon)
    {
        int yAngle = Mathf.RoundToInt(cannon.localEulerAngles.y);
        yAngle = Mathf.Abs(180-yAngle) - 180 + angleClamp;

        return yAngle >= 0;
    }
}