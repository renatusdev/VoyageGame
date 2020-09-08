using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonShot : MonoBehaviour
{
    private void OnParticleCollision(GameObject o)
    {
        List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();
        GetComponent<ParticleSystem>().GetCollisionEvents(o, collisionEvents);

        Vector3 hitPos = Vector3.zero;

        foreach (ParticleCollisionEvent pC in collisionEvents)
            hitPos = pC.intersection;

        Instantiate(GameAssets.i.shipHitParticleSystem, hitPos, Quaternion.identity);
    }
}
