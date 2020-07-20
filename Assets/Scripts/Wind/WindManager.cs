using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindManager : MonoBehaviour
{   
    // Singleton
    public static WindManager instance;

    // The Camera has a wind trail particle system to inform the player of the wind direction.
    public WindParticle trails;

    // The wind direction
    Vector3 direction;

    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        Change(new Vector3(0, 0, 1));
    }   

    public Vector3 getDirection()
    {
        return direction;
    }

    public void Change(Vector3 newWind)
    {   
        direction = newWind.normalized;
        trails.Instantiate();
    }

}
