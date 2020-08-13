using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindParticle : MonoBehaviour
{
    public int windReplay;
    ParticleSystem wind;

    void Start()
    {
        wind = GetComponent<ParticleSystem>();
    }

    public void Play()
    {
        if(isActiveAndEnabled)
            StartCoroutine(PlayTrails(windReplay));
    }

    IEnumerator PlayTrails(int count)
    {
        for (int i = count; i > 0; i--)
        {
            if (!wind.isPlaying)
            {
                wind.transform.forward = WindManager.instance.GetDirection();
                wind.Play();
            }

            yield return new WaitForSeconds(wind.main.duration+wind.main.startLifetime.constantMax);
        }
    }
}
