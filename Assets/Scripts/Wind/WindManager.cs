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


    private void Awake()
    {
        if (instance == null)   
            instance = this;
        else
            Destroy(this);
    }


    private void Start()
    {
        Change(new Vector3(1, 0, 0));
    }

    public Vector3 GetDirection()
    {
        return direction;
    }

    public void Change(Vector3 newWind)
    {   
        direction = newWind.normalized;
        trails.Play();
    }

}
