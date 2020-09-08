using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineShake : MonoBehaviour
{
    public static CinemachineShake instance { get;  private set; }

    private bool isShaking;

    private CinemachineVirtualCamera cam;

    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        isShaking  = false;
        cam = GetComponent<CinemachineVirtualCamera>();
    }

    public void Shake(float intensity, float duration)
    {
        CinemachineBasicMultiChannelPerlin p = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        isShaking = true;
        p.m_AmplitudeGain = intensity;

        StartCoroutine(Shaking(duration, p));
    }

    private IEnumerator Shaking(float duration, CinemachineBasicMultiChannelPerlin p)
    {
        yield return new WaitForSeconds(duration);
        p.m_AmplitudeGain = 0;
        isShaking = false;
    }

    public bool IsActive() { return isShaking; }
}
