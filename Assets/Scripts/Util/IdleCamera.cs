using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleCamera : MonoBehaviour
{
    LTDescr rot;
    
    private void Start()
    {
        rot = LeanTween.rotateY(this.gameObject, -93, 18).setLoopPingPong();
    }

    public void OnDestroy()
    {
        LeanTween.cancel(this.gameObject);
    }
}
