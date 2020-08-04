using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenTool : MonoBehaviour
{
    public AnimationMove anim;
    public bool onEnable;
    public bool isTweening;

    private void OnEnable()
    {
        isTweening = false;

        if (onEnable & !isTweening)
        {
            anim.objToAnimate.transform.localScale = anim.endVector;
            LeanTween.scale(anim.objToAnimate, anim.startVector, anim.duration).setEase(anim.curve).setOnComplete(() => isTweening = false);
            isTweening = true;
        }
    }

    public void Tween()
    {
        if (!isTweening)
        {
            anim.objToAnimate.transform.localScale = anim.endVector;
            LeanTween.scale(anim.objToAnimate, anim.startVector, anim.duration).setEase(anim.curve).setOnComplete(() => isTweening = false);
            isTweening = true;
        }
    }

}

[System.Serializable]
public class AnimationMove
{
    public string name;
    public GameObject objToAnimate;
    public Vector3 startVector;
    public Vector3 endVector;
    public LeanTweenType curve;
    public float duration;
}
