using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonShot : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("oh" + other.name);
    }
}
